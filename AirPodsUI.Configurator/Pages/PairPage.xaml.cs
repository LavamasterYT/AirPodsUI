using AirPodsUI.Configurator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Management;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;

namespace AirPodsUI.Configurator.Pages
{
    /// <summary>
    /// Interaction logic for PairPage.xaml
    /// </summary>
    public partial class PairPage : Page
    {
        List<BluetoothDevices> BluetoothDevices;
        List<USBDevice> USBDevices;

        /// <summary>
        /// Instantiate lists and disable controls
        /// </summary>
        public PairPage()
        {
            InitializeComponent();
            applyUSB.IsEnabled = false;
            usbName.IsEnabled = false;
            btName.IsEnabled = false;
            applyBT.IsEnabled = false;
            BluetoothDevices = new List<BluetoothDevices>();
            USBDevices = new List<USBDevice>();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void SelectedDeviceUSB(object sender, SelectionChangedEventArgs e)
        {
            if (usbDevices.SelectedIndex < 0)
            {
                applyUSB.IsEnabled = false;
                usbName.IsEnabled = false;
            }
            else
            {
                applyUSB.IsEnabled = true;
                usbName.IsEnabled = true;
                usbName.Text = USBDevices[usbDevices.SelectedIndex].DeviceName;
            }
        }
        private void SelectedDeviceBT(object sender, SelectionChangedEventArgs e)
        {
            if (BtDevices.SelectedIndex < 0)
            {
                btName.IsEnabled = false;
                applyBT.IsEnabled = false;
            }
            else
            {
                btName.IsEnabled = true;
                applyBT.IsEnabled = true;
                btName.Text = BluetoothDevices[BtDevices.SelectedIndex].DeviceName;
            }
        }

        private async void RefreshBT(object sender, RoutedEventArgs e)
        {
            BtDevices.Items.Clear();
            BluetoothDevices.Clear();
            try
            {
                DeviceInformationCollection PairedDevices = await DeviceInformation.FindAllAsync(BluetoothDevice.GetDeviceSelectorFromPairingState(true));
                foreach (var i in PairedDevices)
                {
                    BluetoothDevices.Add(new BluetoothDevices { DeviceID = i.Id, DeviceName = i.Name, DeviceType = DeviceTypes.Bluetooth });
                    BtDevices.Items.Add($"{i.Name} ({i.Id})");
                }
                btName.Text = "";

                if (PairedDevices.Count < 1)
                {
                    Helper.Error("Error", "Unable to find any bluetooth devices. Make sure you have paired your devices with this computer before and try again.");
                }
            }
            catch (Exception)
            {
                Helper.Error("Error", "There was a unknown error scanning for bluetooth devices. Contact the developer");
            }
        }

        private void RefreshUSB(object sender, RoutedEventArgs e)
        {
            usbDevices.Items.Clear();
            try
            {
                ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_USBHub");

                foreach (ManagementObject usb in mos.Get())
                {
                    USBDevices.Add(new USBDevice { DeviceID = usb.Properties["DeviceID"].Value.ToString(), DeviceName = usb.Properties["Description"].Value.ToString(), DeviceType = DeviceTypes.USB });
                    usbDevices.Items.Add($"{usb.Properties["Description"].Value.ToString()} ({usb.Properties["DeviceID"].Value.ToString()})");
                }
                usbName.Text = "";
            }
            catch (Exception)
            {
                Helper.Error("Error", "There was a error scanning for USB devices. You might want to contact the developer.");
            }
        }

        private void ApplyBT(object sender, RoutedEventArgs e)
        {
            try
            {
                BluetoothDevices SelectedDevice = BluetoothDevices[BtDevices.SelectedIndex];
                PairedDevicesJson PairedDevices = PairedDevicesJson.FromJson(File.ReadAllText(Helper.PairedDevicesFile));
                PairedDevices.Devices.Add(new Device { DeviceAddress = SelectedDevice.DeviceID, DeviceName = btName.Text, DeviceType = "Bluetooth", TemplateLocation = "" });
                File.WriteAllText(Helper.PairedDevicesFile, PDSerialize.ToJson(PairedDevices));

                MessageBox.Show("Done!", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception)
            {
                Helper.Error("Error", "There was a error writing to the PairedDevices.json file. Please contact the developer.");
            }
        }

        private void ApplyUSB(object sender, RoutedEventArgs e)
        {
           try
            {
                USBDevice SelectedDevice = USBDevices[usbDevices.SelectedIndex];
                PairedDevicesJson PairedDevices = PairedDevicesJson.FromJson(File.ReadAllText(Helper.PairedDevicesFile));
                
                PairedDevices.Devices.Add(new Device { DeviceAddress = SelectedDevice.DeviceID, DeviceName = usbName.Text, DeviceType = "USB", TemplateLocation = "" });
                File.WriteAllText(Helper.PairedDevicesFile, PDSerialize.ToJson(PairedDevices));

                MessageBox.Show("Done!", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception)
            {
                Helper.Error("Error", "There was a error writing to the PairedDevices.json file. Please contact the developer.");
            }
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
