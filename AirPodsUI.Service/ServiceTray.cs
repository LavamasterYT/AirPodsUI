using AirPodsUI.Service.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using Windows.Devices.Bluetooth;

namespace AirPodsUI.Service
{
    public class ServiceTray : ApplicationContext
    {
        private NotifyIcon icon { get; set; }
        private Timer timer;
        private PairedDevicesJson json;
        private List<USBDevice> old;
        private List<USBDevice> newU;

        public ServiceTray()
        {
            icon = new NotifyIcon()
            {
                Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location),
                ContextMenuStrip = new ContextMenuStrip()
                {
                    Visible = true
                },
                Visible = true
            };
            icon.ContextMenuStrip.Items.Add("Exit", null, (sender, e) => Exit());
            icon.ContextMenuStrip.Items.Add("Open Configurator", null, (sender, e) => OpenConfigurator());
            icon.ShowBalloonTip(5000, "Welcome", "Welcome to AirPodsUI, to exit, click on the system tray icon.", ToolTipIcon.Info);

            json = PairedDevicesJson.FromJson(File.ReadAllText(Helper.PairedDevicesFile));

            Setup();

            newU = DeviceSearcher.Search();
            old = DeviceSearcher.Search();

            timer = new Timer();
            timer.Interval = 500;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            newU = DeviceSearcher.Search();

            var newList = newU.Except(old, new USBComparer()).ToList();
            if (newList.Count > 0)
            {
                for (int i = 0; i < json.Devices.Count; i++)
                {
                    if (json.Devices[i].DeviceAddress == newList[0].DeviceID)
                    {
                        Launch(json.Devices[i].TemplateLocation, json.Devices[i].DeviceName);
                        break;
                    }
                }
            }    

            old = Copy(newU);
        }

        private List<USBDevice> Copy(List<USBDevice> input)
        {
            List<USBDevice> result = new List<USBDevice>();

            foreach (var i in input)
            {
                result.Add(i);
            }

            return result;
        }

        public async void Setup()
        {
            foreach (var i in json.Devices)
            {
                if (i.DeviceType == "Bluetooth")
                {
                    BluetoothDevice bd;

                    if (i.DeviceAddress.StartsWith("Bluetooth"))
                    {
                        bd = await BluetoothDevice.FromIdAsync(i.DeviceAddress);
                    }
                    else
                    {
                        bd = await BluetoothDevice.FromBluetoothAddressAsync(ulong.Parse(i.DeviceAddress));
                    }

                    bd.ConnectionStatusChanged += Bd_ConnectionStatusChanged;
                }
            }
        }

        private void Bd_ConnectionStatusChanged(BluetoothDevice sender, object args)
        {
            Console.WriteLine($"Status changed for {sender.DeviceId}!");
            if (sender.ConnectionStatus == BluetoothConnectionStatus.Connected)
            {
                for (int i = 0; i < json.Devices.Count; i++)
                {
                    if (json.Devices[i].DeviceAddress == sender.DeviceId)
                    {
                        Launch(json.Devices[i].TemplateLocation, json.Devices[i].DeviceName);
                        break;
                    }
                }
            }
        }

        private void Launch(string template, string name)
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Process.Start(path + "\\AirPodsUI.exe", $"\"{template}\" \"{name}\"");
        }

        private void Exit()
        {
            Environment.Exit(0);
        }

        private void OpenConfigurator()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Process.Start(path + "\\AirPodsUI.exe");
        }
    }
}