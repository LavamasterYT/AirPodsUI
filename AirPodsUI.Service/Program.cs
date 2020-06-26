using System;
using System.Windows.Forms;

namespace AirPodsUI.Service
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ServiceTray());
        }
    }
}
