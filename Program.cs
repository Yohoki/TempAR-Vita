using System;
using System.Windows.Forms;

namespace TempAR
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (var frmMain1 = new FrmMain())
            {
                Application.Run(frmMain1);
            }
        }
    }
}