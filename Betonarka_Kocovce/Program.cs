using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BetonarkaDL;
using BetonarkaFormDL;

namespace Betonarka_Kocovce
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]
        static void Main()
        {
            if (!Directory.Exists(CsvLayer.dataPath))
            {
                Directory.CreateDirectory(CsvLayer.dataPath);
            }

            Task.Run(() => DataCommunication.ModbusTask());
            Task.Run(() => DataCommunication.ProfinetTask());

            // WinForms app
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DataForm.Form = new Form1();
            Application.Run(DataForm.Form);
        }
    }
}
