using System;
using System.Linq;
using AirPodsUI.Core;
using System.Drawing;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using AirPodsUI.Core.Models;
using System.Collections.Generic;

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

            TrayService.ContextMenuStrip.Items.Add("Open Settings", null, (sender, e) => OpenSettings());
            TrayService.ContextMenuStrip.Items.Add("Refresh", null, (sender, e) => Refresh());
            TrayService.ContextMenuStrip.Items.Add("Exit", null, (sender, e) => Exit());

            offset = 60;
            busy = false;

            Logger.Log("Initialized tray");
        }

        public void Refresh()
        {
            busy = true;
            devices = DevicesJson.GetDevices();
            if (devices.Count <= 0)
            {
                MessageBox.Show("There are no devices added, please run the configurator to add devices.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(-1);
            }

            Settings settings = new Settings();

            lastTick = PNP.GetPNPDevices();

            offset = settings.Offset;

            settings.Dispose();
            busy = false;
        }

        public void OpenSettings()
        {
            Process.Start("AirPodsUI.Settings.exe");
        }

        public void Setup()
        {
            try
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
            catch (Exception e)
            {
                Logger.Log("An error occured trying to setup the tray service", e);
                MessageBox.Show("An error occured trying to setup the tray service", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Exit()
        {
            Logger.Log("Exiting, goodbye!");

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

                if (differences.Count > 0)
                {
                    Logger.Log($"Found {differences.Count} new devices");
                }

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
            Logger.Log($"Showing toast for device \"{dev.Identifier}\" with an ID of \"{dev.Identifier}\"");

            ProcessStartInfo pToast = new ProcessStartInfo()
            {
                FileName = $"{AppDomain.CurrentDomain.BaseDirectory}AirPodsUI.Toast.exe",
                Arguments = $"--offset {offset} --name \"{dev.Name}\""
            };

            if (dev.DarkMode)
            {
                pToast.Arguments += " --dark-mode";
            }

            Logger.Log($"Launching \"{pToast.FileName}\" with arguments \"{pToast.Arguments}\"");
            Process.Start(pToast);
        }
    }
}
