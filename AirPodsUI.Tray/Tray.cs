using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;
using AirPodsUI.Core.Models;
using System.Linq;
using AirPodsUI.Core;
using System.Diagnostics;

namespace AirPodsUI.Tray
{
    class Tray : ApplicationContext
    {
        public NotifyIcon TrayService;

        private System.Threading.Timer timer;
        private List<Device> devices;

        private List<PnPDevice> lastTick;
        private List<PnPDevice> curTick;

        private int offset;

        private bool busy;

        public Tray()
        {
            TrayService = new NotifyIcon()
            {
                Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location),
                ContextMenuStrip = new ContextMenuStrip()
                {
                    Visible = true
                },
                Visible = false
            };

            TrayService.ContextMenuStrip.Items.Add("Exit", null, (sender, e) => Exit());

            offset = 60;
            busy = false;
        }

        public void Setup()
        {
            devices = DevicesJson.GetDevices();

            if (devices.Count <= 0)
            {
                MessageBox.Show("There are no devices added, please run the configurator to add devices.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(-1);
            }

            Settings settings = new Settings();

            lastTick = PNP.GetPNPDevices();

            timer = new System.Threading.Timer(OnTick, null, 0, settings.RefreshRate);
            offset = settings.Offset;

            settings.Dispose();

            TrayService.Visible = true;
        }

        public void Exit()
        {
            TrayService.Visible = false;
            TrayService.Dispose();
            timer.Dispose();
            this.ExitThreadCore();
        }

        public void OnTick(object stateInfo)
        {
            if (!busy)
            {
                busy = true;
                curTick = PNP.GetPNPDevices();

                List<PnPDevice> differences = curTick.Where((i) =>
                {
                    return !(lastTick.Contains(i));
                }).ToList();

                foreach (var i in differences)
                {
                    foreach (var j in devices)
                    {
                        if (j.Identifier == i.PNPDeviceID)
                        {
                            ShowToast(j);
                        }
                    }
                }

                lastTick = new List<PnPDevice>(curTick);
                busy = false;
            }
        }

        public void ShowToast(Device dev)
        {
            ProcessStartInfo pToast = new ProcessStartInfo()
            {
                FileName = $"{AppDomain.CurrentDomain.BaseDirectory}AiPodsUI.Toast.exe",
                Arguments = $"--offset {offset} --name \"{dev.Name}\" --type usb"
            };

            if (dev.DarkMode)
            {
                pToast.Arguments += " --dark-mode";
            }

            Debug.WriteLine($"Launching \"{pToast.FileName}\" with arguments \"{pToast.Arguments}\"");
            Process.Start(pToast);
        }
    }
}
