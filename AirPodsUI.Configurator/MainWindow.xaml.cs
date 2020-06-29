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
using Newtonsoft.Json.Linq;
using System.Reflection;

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

        public MainWindow()
        {
            InitializeComponent();

            // Set the page to the read me 
            MainFrame.NavigationService.Navigate(new MainPage());

            if (int.Parse(Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion").GetValue("ReleaseId").ToString()) < 1903)
            {
                MessageBox.Show("This program detects that you are running a version of Windows that is not supported by this program. Please update to the latest version in order to continue.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                Environment.Exit(0);
            }

            // Get system app theme and set app theme based on values
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            if (int.TryParse(reg.GetValue("AppsUseLightTheme").ToString(), out int result))
            {
                if (result == 0)
                {
                    TitleBarBackground = System.Windows.Media.Brushes.Black;
                    ResourceLocator.SetColorScheme(Application.Current.Resources, ResourceLocator.DarkColorScheme);
                }
                else
                {
                    TitleBarBackground = System.Windows.Media.Brushes.White;
                    ResourceLocator.SetColorScheme(Application.Current.Resources, ResourceLocator.LightColorScheme);
                }
            }

            deviceListBind = new List<DeviceListModel>();
            jsonWatcher = new FileSystemWatcher();

            this.Dispatcher.Invoke(RefreshDeviceList);

            // Watch for changes to the JSON file to refresh the list
            jsonWatcher.Path = Helper.AirPodsUIFolder;
            jsonWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.LastAccess;
            jsonWatcher.Filter = "*.json";

            jsonWatcher.Changed += JsonWatcher_Changed;

            jsonWatcher.EnableRaisingEvents = true;

            // Check for updates
            JObject data = JObject.Parse("https://api.github.com/repos/LavamasterYT/AirPods-for-Windows/releases/latest");
            Version latest = Version.Parse((string)data.SelectToken("tag_name"));
            Version current = Assembly.GetExecutingAssembly().GetName().Version;
            if (current.CompareTo(latest) > 0)
            {
                MessageBox.Show("There is a new version available to download! Please go to the project repository to update!", "Notice", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }

            Show();
        }

        private void JsonWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            this.Dispatcher.Invoke(RefreshDeviceList);
        }
        private async void RefreshDeviceList()
        {
            // Wait in case you recently added a device via this app
            await Task.Delay(500);

            // Read json file and add to list
            try
            {
                jsonFile = PairedDevicesJson.FromJson(File.ReadAllText(Helper.PairedDevicesFile));
                deviceListBind.Clear();
                Devices.Items.Clear();
                foreach (var i in jsonFile.Devices)
                {
                    deviceListBind.Add(new DeviceListModel { DeviceName = i.DeviceName, ImageSource = new Uri((i.DeviceType == "Bluetooth") ? "pack://application:,,,/Assets/bluetooth.png" : "pack://application:,,,/Assets/usb.png") });
                    Devices.Items.Add(new DeviceListModel { DeviceName = i.DeviceName, ImageSource = new Uri((i.DeviceType == "Bluetooth") ? "pack://application:,,,/Assets/bluetooth.png" : "pack://application:,,,/Assets/usb.png") });
                }
            }
            catch (Exception)
            {
                Helper.Error("Error", "Unable to read PairedDevices.json. Please make sure you have not modified it. If this error still persists, try running as administrator or contact the developer.");
                deviceListBind.Clear();
                Devices.Items.Clear();
            }
        }

        private void ShowPairPage(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new PairPage());
        }

        private void DevicesIndexChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (Devices.SelectedIndex < 0)
            {
                MainFrame.NavigationService.Navigate(new MainPage());
                return;
            }

            MainFrame.NavigationService.Navigate(new DeviceConfigPage(jsonFile, Devices.SelectedIndex));
        }
    }
}