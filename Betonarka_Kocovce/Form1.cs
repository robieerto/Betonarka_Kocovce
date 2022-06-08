using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Betonarka_Kocovce
{
    public partial class Form1 : Form
    {
        List<double> dataModbus;
        List<int> dataProfinet;

        int timer = 0;

        public Form1()
        {
            InitializeComponent();
        }

        public void ReadValues()
        {
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

            // palety
            dataProfinet = ProfinetS7.ReadData();
            if (dataProfinet != null)
            {
                textBox8.Text = Convert.ToString(dataProfinet[0]);
                textBox9.Text = Convert.ToString(dataProfinet[1]);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer++;

            if (timer >= 10)
            {
                ReadValues();
                timer = 0;
            }
        }
    }
}
