using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S7.Net;

namespace BetonarkaDL
{
    public static class ProfinetS7
    {
        private static string ipAddr = "213.215.84.85";
        private static int ipPort = 8901;
        //private static string ipAddr = "192.168.1.10";
        //private static int ipPort = 102;
        private static short rack = 0;
        private static short slot = 1;
        private static int db = 2110;
        private static Plc plc = new Plc(CpuType.S71200, ipAddr, ipPort, rack, slot);

        public static bool isConnected;
        public static bool readyToSave;

        public static List<int> ReadData()
        {
            try
            {
                if (plc.IsConnected == false)
                {
                    try
                    {
                        plc.Open();
                    }
                    catch (Exception)
                    {
                        isConnected = false;
                        return null;
                    }
                }
                isConnected = true;

                //var bitArray = (bool[])plc.Read(DataType.DataBlock, db, 0, VarType.Bit, 1, 0);
                //var data = ((int[])plc.Read(DataType.DataBlock, db, 2, VarType.Word, 2)).ToList();
                var data = new List<int>();
                int firstValue = (ushort)plc.Read("DB2110.DBW2");
                int secondValue = (ushort)plc.Read("DB2110.DBW4");
                data.Add(firstValue);
                data.Add(secondValue);

                bool readyBit = (bool)plc.Read("DB2110.DBX0.0");
                if (readyBit == true)
                {
                    readyToSave = true;
                    //plc.WriteBit(DataType.DataBlock, db, 0, 0, false);
                    plc.Write("DB2110.DBX0.0", false);
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
