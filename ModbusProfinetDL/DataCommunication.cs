using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetonarkaFormDL;

namespace BetonarkaDL
{
    public static class DataCommunication
    {
        static List<double> dataModbus, dataModbusLast;
        static List<int> dataProfinet;

        // miesacky
        public static void ModbusTask()
        {
            while (true)
            {
                // save last value
                if (dataModbus != null)
                {
                    dataModbusLast = new List<double>(dataModbus);
                }
                // velka miesacka
                dataModbus = ModbusTCP.MasterReadDoubleWords(618, 4);
                if (dataModbus != null)
                {
                    Library.WriteLastTimeModbus();
                    // mala miesacka
                    var nextData = ModbusTCP.MasterReadDoubleWords(718, 3);
                    if (nextData != null)
                    {
                        dataModbus.AddRange(nextData);
                        DataForm.FillModbus(dataModbus);
                    }
                }

                // check and save data
                bool willSaveDataModbus = false;
                if (dataModbus != null && dataModbusLast != null)
                {
                    for (int i = 0; i < dataModbus.Count; i++)
                    {
                        if (dataModbus[i] < dataModbusLast[i])
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

                Task.Delay(2000);
            }
        }

        public static void ProfinetTask()
        {
            while (true)
            {
                dataProfinet = ProfinetS7.ReadData();
                if (dataProfinet != null)
                {
                    Library.WriteLastTimeProfinet();
                    DataForm.FillProfinet(dataProfinet);
                    if (ProfinetS7.readyToSave == true)
                    {
                        ProfinetS7.readyToSave = false;
                        CsvLayer.SavePalety(dataProfinet);
                    }
                }

                Task.Delay(2000);
            }
        }
    }
}
