using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BetonarkaFormDL
{
    public static class DataForm
    {
        public static dynamic Form;

        public static void FillModbus(List<double> dataModbus)
        {
            if (Form != null)
            {
                Form.textBox1.Text = Convert.ToString(dataModbus[0]); // cement
                Form.textBox2.Text = Convert.ToString(dataModbus[1]); // cement biely
                Form.textBox3.Text = Convert.ToString(dataModbus[2]); // struska
                Form.textBox4.Text = Convert.ToString(dataModbus[3]); // popol
                Form.textBox5.Text = Convert.ToString(dataModbus[4]); // cement
                Form.textBox6.Text = Convert.ToString(dataModbus[5]); // cement biely
                Form.textBox7.Text = Convert.ToString(dataModbus[6]); // struska
                Form.textBox10.Text = DateTime.Now.ToString();
                Form.textBox12.Text = "Pripojené";
            }
        }

        public static void FillProfinet(List<int> dataProfinet)
        {
            if (Form != null)
            {
                Form.textBox8.Text = Convert.ToString(dataProfinet[0]);
                Form.textBox9.Text = Convert.ToString(dataProfinet[1]);
                Form.textBox11.Text = DateTime.Now.ToString();
                Form.textBox13.Text = "Pripojené";
            }
        }
    }
}
