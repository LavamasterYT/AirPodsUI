using System;
using System.Windows;
using AirPodsUI.Core;
using ModernWpf.Controls;
using AirPodsUI.Settings.Pages;
using ModernWpf.Media.Animation;

namespace AirPodsUI.Settings
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            App.OnDevicesChanged += OnDevicesChanged;
        }

        private void OnDevicesChanged(object sender, EventArgs e)
        {
            RefreshDevices();
        }

        private void RefreshDevices()
        {
            Logger.Log(LogType.Information, "Refreshing devices");

            while (sPNPDevices.MenuItems.Count > 1)
            {
                sPNPDevices.MenuItems.RemoveAt(0);
            }

            App.NavPages.Clear();
            App.ResetNavPages();

            foreach (var i in App.Devices)
            {
                NavigationViewItem dev = new NavigationViewItem();
                dev.Tag = i.Identifier;
                dev.Content = i.Name;
                App.NavPages.Add(i.Identifier, typeof(DeviceSettings));

                sPNPDevices.MenuItems.Insert(0, dev);
            }
        }

        private void OnNavViewChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            try
            {
                Logger.Log(LogType.Information, "Changing pages from navigation view");

                string tag = ((NavigationViewItem)args.SelectedItem).Tag.ToString();
                DeviceSettings.ID = tag;

                sPageFrame.Navigate(App.NavPages[tag], tag, new EntranceNavigationTransitionInfo());
            }
            catch (Exception e)
            {
                Logger.Log(LogType.Warning, "Unable to change pages!", e);
            }
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            RefreshDevices();
        }
    }
}
