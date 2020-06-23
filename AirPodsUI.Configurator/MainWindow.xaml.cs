using AdonisUI;
using System.Windows;
using Microsoft.Win32;
using AdonisUI.Controls;
using AirPodsUI.Configurator.Pages;
using System.IO;
using AirPodsUI.Configurator.Models;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace AirPodsUI.Configurator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : AdonisWindow
    {
        FileSystemWatcher jsonWatcher;
        PairedDevicesJson jsonFile;
        List<DeviceListModel> _deviceListBind;

        List<DeviceListModel> deviceListBind
        {
            get => _deviceListBind;
            set
            {
                _deviceListBind = value;
                Devices.ItemsSource = _deviceListBind;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            // Set the page to the read me 
            MainFrame.NavigationService.Navigate(new MainPage());

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
            jsonFile = PairedDevicesJson.FromJson(File.ReadAllText(Helper.PairedDevicesFile));
            deviceListBind.Clear();
            foreach (var i in jsonFile.Devices)
            {
                deviceListBind.Add(new DeviceListModel { DeviceName = i.DeviceName, ImageSource = new Uri((i.DeviceType == "Bluetooth") ? "pack://application:,,,/Assets/bluetooth.png" : "pack://application:,,,/Assets/usb.png") });
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

            MainFrame.NavigationService.Navigate(new DeviceConfigPage());
        }
    }
}