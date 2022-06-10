using System;
using System.Collections.Generic;

namespace Betonarka_Kocovce
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        List<double> dataModbus;
        List<double> dataModbusLast;
        List<int> dataProfinet;
        bool profinetConnected = true;

        int timer = 0;

        public Form1()
        {
            InitializeComponent();
            this.Text = "Betonárka Kočovce";
            this.Select();

            ReadModbus();
        }

        // miesacky
        public void ReadModbus()
        {
            if (dataModbus != null)
            {
                dataModbusLast = new List<double>(dataModbus);
            }

            // velka miesacka
            dataModbus = ModbusTCP.MasterReadDoubleWords(618, 4);
            textBox1.Text = Convert.ToString(dataModbus[0]); // cement
            textBox2.Text = Convert.ToString(dataModbus[1]); // cement biely
            textBox3.Text = Convert.ToString(dataModbus[2]); // struska
            textBox4.Text = Convert.ToString(dataModbus[3]); // popol

            // mala miesacka
            dataModbus.AddRange(ModbusTCP.MasterReadDoubleWords(718, 3));
            textBox5.Text = Convert.ToString(dataModbus[4]); // cement
            textBox6.Text = Convert.ToString(dataModbus[5]); // cement biely
            textBox7.Text = Convert.ToString(dataModbus[6]); // struska
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
            }
        }

        // palety
        public void ReadProfinet()
        {
            if (profinetConnected)
            {
                dataProfinet = ProfinetS7.ReadData();
            }
            if (dataProfinet != null)
            {
                textBox8.Text = Convert.ToString(dataProfinet[0]);
                textBox9.Text = Convert.ToString(dataProfinet[1]);
            }
            else
            {
                profinetConnected = false;
            }
        }

        public void CheckAndSaveProfinet()
        {
            if (ProfinetS7.readyToSave == true)
            {
                ProfinetS7.readyToSave = false;
                CsvLayer.SavePalety(dataProfinet);
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
