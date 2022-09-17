using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BetonarkaDL;

namespace Betonarka_Kocovce
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        int timer = 0;

        public Form1()
        {
            InitializeComponent();
            this.Text = "Betonárka Kočovce";
            this.Select();
            textBox12.Text = "Nadväzujem spojenie";
            textBox13.Text = "Nadväzujem spojenie";
            CheckForIllegalCrossThreadCalls = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer++;

            if (timer >= 1)
            {
                timer = 0;
            }
        }
    }
}
