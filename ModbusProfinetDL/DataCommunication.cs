using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetonarkaDL
{
    public static class DataCommunication
    {
        static List<double> dataModbus, dataModbusLast;
        static List<int> dataProfinet;
        static bool modbusTaskIsRunning, profinetTaskIsRunning;

        // miesacky
        public static void ModbusTask()
        {
            while (true)
            {
                modbusTaskIsRunning = true;
                // save last value
                if (dataModbus != null)
                {
                    dataModbusLast = new List<double>(dataModbus);
                }
                // velka miesacka
                dataModbus = ModbusTCP.MasterReadDoubleWords(618, 4);
                if (dataModbus != null)
                {
                    // mala miesacka
                    var nextData = ModbusTCP.MasterReadDoubleWords(718, 3);
                    if (nextData != null)
                    {
                        dataModbus.AddRange(nextData);
                    }
                }

                // check and save data
                bool willSaveDataModbus = false;
                if (dataModbus != null && dataModbusLast != null)
                {
                    for (int i = 0; i < dataModbus.Count; i++)
                    {
                        if (true)// (dataModbus[i] < dataModbusLast[i])
                        {
                            willSaveDataModbus = true;
                            break;
                        }
                    }
                }
                if (willSaveDataModbus)
                {
                    CsvLayer.SaveMiesacky(dataModbus);
                }

                modbusTaskIsRunning = false;
                Task.Delay(2000);
            }
        }

        public static void ProfinetTask()
        {
            while (true)
            {
                profinetTaskIsRunning = true;
                dataProfinet = ProfinetS7.ReadData();
                if (dataProfinet != null && ProfinetS7.readyToSave == true)
                {
                    ProfinetS7.readyToSave = false;
                    CsvLayer.SavePalety(dataProfinet);
                }

                profinetTaskIsRunning = false;
                Task.Delay(2000);
            }
        }
    }
}
