using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S7.Net;

namespace Betonarka_Kocovce
{
    public static class ProfinetS7
    {
        public static string ipAddr = "213.215.84.85";
        public static int ipPort = 8901;
        public static short rack = 0;
        public static short slot = 1;
        public static int startAddr = 2110;

        public static bool readyToSave;

        public static Plc plc = new Plc(CpuType.S71200, ipAddr, ipPort, rack, slot);

        public static List<int> ReadData()
        {
            try
            {
                if (plc.IsConnected == false)
                {
                    plc.Open();
                    if (plc.IsConnected == false)
                        return null;
                }

                var bitArray = (bool[])plc.Read(DataType.DataBlock, startAddr, 0, VarType.Bit, 1, 0);
                var data = ((int[])plc.Read(DataType.DataBlock, startAddr, 2, VarType.DWord, 2)).ToList();
                bool readyBit = bitArray[0];
                if (readyBit == true)
                {
                    readyToSave = true;
                    plc.WriteBit(DataType.DataBlock, 2110, 0, 0, false);
                }
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
