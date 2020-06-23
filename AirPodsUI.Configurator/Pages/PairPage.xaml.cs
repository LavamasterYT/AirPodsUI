using AirPodsUI.Configurator.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AirPodsUI.Configurator.Pages
{
    /// <summary>
    /// Interaction logic for PairPage.xaml
    /// </summary>
    public partial class PairPage : Page
    {
        List<BluetoothDevice> BluetoothDevices;
        List<USBDevice> USBDevices;

        public PairPage()
        {
            InitializeComponent();
            BluetoothDevices = new List<BluetoothDevice>();
            USBDevices = new List<USBDevice>();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void SelectedDeviceUSB(object sender, SelectionChangedEventArgs e)
        {

        }
        private void SelectedDeviceBT(object sender, SelectionChangedEventArgs e)
        {

        }

        private void RefreshBT(object sender, RoutedEventArgs e)
        {

        }

        private void RefreshUSB(object sender, RoutedEventArgs e)
        {

        }

        private void ApplyBT(object sender, RoutedEventArgs e)
        {

        }

        private void ApplyUSB(object sender, RoutedEventArgs e)
        {

        }

        private void Page_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            try
            {
                if (e.Delta < 0)
                {
                    mainScrollBar.ScrollToVerticalOffset(mainScrollBar.VerticalOffset + 30);
                }
                else if (e.Delta > 0)
                {
                    mainScrollBar.ScrollToVerticalOffset(mainScrollBar.VerticalOffset - 30);
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
