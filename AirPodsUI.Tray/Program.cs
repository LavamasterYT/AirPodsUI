using System;
using System.Windows.Forms;

namespace AirPodsUI.Tray
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Tray tray = new Tray();
            tray.Setup();
            Application.Run(tray);
        }
    }
}
