using AdonisUI;
using System.Windows;
using Microsoft.Win32;
using AdonisUI.Controls;
using MessageBox = System.Windows.MessageBox;
using AirPodsUI.Configurator.Pages;
using System.IO;
using AirPodsUI.Configurator.Models;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using RegistryUtils;
using Windows.Security.Authentication.Identity.Core;
using Serilog;

namespace AirPodsUI.Configurator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : AdonisWindow
    {
        FileSystemWatcher jsonWatcher;
        PairedDevicesJson jsonFile;

        List<DeviceListModel> deviceListBind { get; set; }

        RegistryMonitor monitor;
        bool changed = false;

        public MainWindow()
        {
            InitializeComponent();

            // Set the page to the read me 
            MainFrame.NavigationService.Navigate(new MainPage());
            Log.Information("Loading read me...");

            Log.Information("Getting Windows 10 version.");
            if (int.Parse(Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion").GetValue("ReleaseId").ToString()) < 1903)
            {
                Log.Error("Windows version is unsupported.");
                MessageBox.Show("This program detects that you are running a version of Windows that is not supported by this program. Please update to the latest version in order to continue.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                Environment.Exit(0);
            }

            Log.Information("Changing theme.");
            ChangeTheme();

            deviceListBind = new List<DeviceListModel>();
            jsonWatcher = new FileSystemWatcher();

            this.Dispatcher.Invoke(RefreshDeviceList);

            // Watch for changes to the JSON file to refresh the list
            Log.Information("Subscribing to FileWatcher.");
            jsonWatcher.Path = Helper.AirPodsUIFolder;
            jsonWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.LastAccess;
            jsonWatcher.Filter = "*.json";
            jsonWatcher.Changed += JsonWatcher_Changed;
            jsonWatcher.EnableRaisingEvents = true;

            // Watch for changes in theme
            Log.Information("Watching for registry changes.");
            monitor = new RegistryMonitor(RegistryHive.CurrentUser, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            monitor.RegChanged += Monitor_RegChanged;
            monitor.RegChangeNotifyFilter = RegChangeNotifyFilter.Value;
            monitor.Start();

            Log.Information("Showing main window.");
            Show();
        }

        private void Monitor_RegChanged(object sender, EventArgs e)
        {
            Log.Information("Changing theme because registry changed.");
            if (!changed)
            {
                changed = true;
            }
            else
            {
                changed = false;
                ChangeTheme();
            }
        }

        private void ChangeTheme()
        {
            this.Dispatcher.Invoke(() => 
            {
                // Get system app theme and set app theme based on values
                RegistryKey reg = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize");
                if (int.TryParse(reg.GetValue("AppsUseLightTheme").ToString(), out int result))
                {
                    Log.Information("Got selected theme from user, applying.");
                    if (result == 0)
                    {
                        Log.Information("Applying dark mode.");
                        TitleBarBackground = System.Windows.Media.Brushes.Black;
                        ResourceLocator.SetColorScheme(Application.Current.Resources, ResourceLocator.DarkColorScheme);
                    }
                    else
                    {
                        Log.Information("Applying light mode.");
                        TitleBarBackground = System.Windows.Media.Brushes.White;
                        ResourceLocator.SetColorScheme(Application.Current.Resources, ResourceLocator.LightColorScheme);
                    }
                }
                reg.Close();
                reg.Dispose();
            });
        }

        private void JsonWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            Log.Information("JSON file changed, refreshing device list.");
            this.Dispatcher.Invoke(RefreshDeviceList);
        }
        private async void RefreshDeviceList()
        {
            // Wait in case you recently added a device via this app
            await Task.Delay(500);

            // Read json file and add to list
            try
            {
                Log.Information("Reading information from PairedDevices.json");
                jsonFile = PairedDevicesJson.FromJson(File.ReadAllText(Helper.PairedDevicesFile));
                deviceListBind.Clear();
                Devices.Items.Clear();
                foreach (var i in jsonFile.Devices)
                {
                    Log.Information($"Adding {i.DeviceName} to the list.");
                    deviceListBind.Add(new DeviceListModel { DeviceName = i.DeviceName, ImageSource = new Uri((i.DeviceType == "Bluetooth") ? "pack://application:,,,/Assets/bluetooth.png" : "pack://application:,,,/Assets/usb.png") });
                    Devices.Items.Add(new DeviceListModel { DeviceName = i.DeviceName, ImageSource = new Uri((i.DeviceType == "Bluetooth") ? "pack://application:,,,/Assets/bluetooth.png" : "pack://application:,,,/Assets/usb.png") });
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "An error has occured trying to refresh device list.");
                Helper.Error("Error", "Unable to read PairedDevices.json. Please make sure you have not modified it. If this error still persists, try running as administrator or contact the developer.");
                deviceListBind.Clear();
                Devices.Items.Clear();
            }
        }

        private void ShowPairPage(object sender, RoutedEventArgs e)
        {
            Log.Information("Going to the pair page.");
            MainFrame.NavigationService.Navigate(new PairPage());
        }

        private void DevicesIndexChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Log.Information("User has selected a device.");
            if (Devices.SelectedIndex < 0)
            {
                MainFrame.NavigationService.Navigate(new MainPage());
                return;
            }

            MainFrame.NavigationService.Navigate(new DeviceConfigPage(jsonFile, Devices.SelectedIndex));
        }

        private void StartServiceBtn(object sender, RoutedEventArgs e)
        {
            try
            {
                Log.Information("Stating the AirPodsUI service.");
                Process.Start($"{Helper.AppDirectory}\\AirPodsUI.Configurator.exe");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "We couldnt start the damn service what a shame.");
                Helper.Error("Error", "Unable to start service.");
            }
        }

        private void AdonisWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Log.Information("Sucks to have you go, have a great day!");
            Log.CloseAndFlush();
            monitor.Stop();
            monitor.Dispose();
        }
    }
}