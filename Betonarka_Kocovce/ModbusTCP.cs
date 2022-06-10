using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NModbus;

namespace Betonarka_Kocovce
{
    public static class ModbusTCP
    {
        public static string ipAddr = "213.215.84.85";
        public static int ipPort = 8881;
        public static byte slaveId = 0;

        public static List<double> MasterReadDoubleWords(ushort startAddress, ushort numDoubleWords)
        {
            using (TcpClient client = new TcpClient(ipAddr, ipPort))
            {
                var factory = new ModbusFactory();
                IModbusMaster master = factory.CreateMaster(client);

                ushort numRegisters = (ushort)(numDoubleWords * 2); // double word is across 2 registers
                ushort[] registers = master.ReadHoldingRegisters(slaveId, startAddress, numRegisters);

                List<double> recvData = new List<double>();

                for (int i = 0; i < numRegisters; i += 2)
                {
                    double recvValue = registers[i] | (registers[i + 1] << 16);
                    recvData.Add(recvValue / 10); // 1 decimal value
                }

                return recvData;
            }
        }
    }
}
