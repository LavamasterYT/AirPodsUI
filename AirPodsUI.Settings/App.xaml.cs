using AirPodsUI.Core.Models;
using AirPodsUI.Settings.Pages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AirPodsUI.Settings
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Dictionary<string, Type> NavPages;
        public static event EventHandler OnDevicesChanged;
        public static List<Device> Devices;

        public App()
        {
            NavPages = new Dictionary<string, Type>();
            Devices = new List<Device>();
            Devices = DevicesJson.GetDevices();
            OnDevicesChanged?.Invoke(this, new EventArgs());
        }

        public static void InvokeDeviceChange(object sender)
        {
            OnDevicesChanged?.Invoke(sender, new EventArgs());
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            ResetNavPages();
        }

        public static void ResetNavPages()
        {
            NavPages.Add("tray_settings", typeof(TraySettings));
            NavPages.Add("add_device", typeof(AddDevice));
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            
        }
    }
}
