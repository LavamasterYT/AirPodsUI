using AirPodsUI.Settings.Pages;
using ModernWpf.Controls;
using ModernWpf.Media.Animation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
                string tag = ((NavigationViewItem)args.SelectedItem).Tag.ToString();
                DeviceSettings.ID = tag;

                sPageFrame.Navigate(App.NavPages[tag], tag, new EntranceNavigationTransitionInfo());
            }
            catch (Exception) { }
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            RefreshDevices();
        }
    }
}
