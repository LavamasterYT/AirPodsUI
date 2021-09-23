using System;
using AirPodsUI.Core;
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

            Logger.CreateLog("AirPods Tray");
            Logger.Log("Created logger! :p");

            Tray tray = new Tray();
            tray.Setup();
            Application.Run(tray);
        }
    }
}
