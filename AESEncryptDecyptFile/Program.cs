using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace AESEncryptDecyptFile
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (!File.Exists("configName.cfg.aes") || !File.Exists("configEmail.cfg.aes"))
            {
                Application.Run(new Form2());
            }
            else
            {
                Application.Run(new Form1());
            }
        }
    }
}
