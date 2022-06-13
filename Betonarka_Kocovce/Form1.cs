using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Betonarka_Kocovce
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        List<double> dataModbus, dataModbusLast;
        List<int> dataProfinet;
        bool readingModbus, readingProfinet;

        int timer = 0;

        public Form1()
        {
            InitializeComponent();
            this.Text = "Betonárka Kočovce";
            this.Select();

            textBox12.Text = "Nadväzujem spojenie";
            textBox13.Text = "Nadväzujem spojenie";

            ReadModbus();
            ReadProfinet();
        }

        // miesacky
        public void ReadModbus()
        {
            if (readingModbus == false)
            {
                readingModbus = true;
                if (dataModbus != null)
                {
                    dataModbusLast = new List<double>(dataModbus);
                }
                Task.Run(() =>
                {
                    // velka miesacka
                    dataModbus = ModbusTCP.MasterReadDoubleWords(618, 4);
                    if (dataModbus != null)
                    {
                        textBox1.Text = Convert.ToString(dataModbus[0]); // cement
                        textBox2.Text = Convert.ToString(dataModbus[1]); // cement biely
                        textBox3.Text = Convert.ToString(dataModbus[2]); // struska
                        textBox4.Text = Convert.ToString(dataModbus[3]); // popol

                        // mala miesacka
                        var nextData = ModbusTCP.MasterReadDoubleWords(718, 3);
                        if (nextData != null)
                        {
                            dataModbus.AddRange(nextData);
                            textBox5.Text = Convert.ToString(dataModbus[4]); // cement
                            textBox6.Text = Convert.ToString(dataModbus[5]); // cement biely
                            textBox7.Text = Convert.ToString(dataModbus[6]); // struska
                            textBox12.Text = "Pripojené";
                        }
                        else
                        {
                            textBox12.Text = "Nepripojené";
                        }
                    }
                    else
                    {
                        textBox12.Text = "Nepripojené";
                    }
                    readingModbus = false;
                });
            }

        }

        public void CheckAndSaveModbus()
        {
            bool willSaveDataModbus = false;
            if (dataModbus != null && dataModbusLast != null)
            {
                for (int i = 0; i < dataModbus.Count; i++)
                {
                    if (dataModbus[i] == 0 && dataModbusLast[i] != 0)
                    {
                        willSaveDataModbus = true;
                        break;
                    }
                }
            }
            if (willSaveDataModbus)
            {
                CsvLayer.SaveMiesacky(dataModbus);
                textBox10.Text = CsvLayer.lastTimeMiesacky;
            }
        }

        // palety
        public void ReadProfinet()
        {
            if (readingProfinet == false)
            {
                readingProfinet = true;
                Task.Run(() =>
                {
                    dataProfinet = ProfinetS7.ReadData();
                    if (dataProfinet != null)
                    {
                        textBox8.Text = Convert.ToString(dataProfinet[0]);
                        textBox9.Text = Convert.ToString(dataProfinet[1]);
                        textBox13.Text = "Pripojené";
                    }
                    else
                    {
                        if (ProfinetS7.isConnected == false)
                        {
                            textBox13.Text = "Nepripojené";
                        }
                        else
                        {
                            textBox13.Text = "Pripojené, nemožno vyčítať dáta";
                        }
                    }
                    readingProfinet = false;
                });
            }

        }

        public void CheckAndSaveProfinet()
        {
            if (ProfinetS7.readyToSave == true)
            {
                ProfinetS7.readyToSave = false;
                CsvLayer.SavePalety(dataProfinet);
                textBox11.Text = CsvLayer.lastTimePalety;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer++;

            if (timer >= 1)
            {
                timer = 0;
                ReadModbus();
                CheckAndSaveModbus();
                ReadProfinet();
                CheckAndSaveProfinet();
            }
        }
    }
}
