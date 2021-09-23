using System;
using System.Windows;
using AirPodsUI.Core;
using AirPodsUI.Core.Models;
using AirPodsUI.Settings.Pages;
using System.Collections.Generic;

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
            Exception exLog = Logger.CreateLog("AirPodsUI Settings");
            if (exLog != null)
            {
                MessageBox.Show("Unable to create logging component. Logging will be unavailable.\n\n" + exLog.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            Logger.Log(LogType.Information, "Created logger!");

            try
            {
                NavPages = new Dictionary<string, Type>();
                Devices = new List<Device>();
                Devices = DevicesJson.GetDevices();
                OnDevicesChanged?.Invoke(this, new EventArgs());
            }
            catch (Exception e)
            {
                Logger.Log(LogType.Error, "Unable to get devices!", e);
                MessageBox.Show("There was an error trying to get the devices. Please see the log for more details. Shutting down.\n\n" + Logger.LogFile, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }

        public static void InvokeDeviceChange(object sender)
        {
            OnDevicesChanged?.Invoke(sender, new EventArgs());
            Logger.Log(LogType.Information, "Invoked Device Change");
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            Logger.Log(LogType.Information, "Resetting navigation pages");
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

        private void OnException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.Log(LogType.Error, "An unknown error occured and the application had to close!", e.Exception);
            MessageBox.Show("An unknown error occured and the application had to close! Please check the log file for details.\n\n" + Logger.LogFile, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
