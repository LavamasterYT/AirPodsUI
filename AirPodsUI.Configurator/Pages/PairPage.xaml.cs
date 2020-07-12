using AirPodsUI.Configurator.Models;
using Serilog;
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
            Log.Information("Setting variables.");
            applyUSB.IsEnabled = false;
            usbName.IsEnabled = false;
            btName.IsEnabled = false;
            applyBT.IsEnabled = false;
            BluetoothDevices = new List<BluetoothDevices>();
            USBDevices = new List<USBDevice>();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Log.Information("Loaded page.");
        }

        private void SelectedDeviceUSB(object sender, SelectionChangedEventArgs e)
        {
            Log.Information("Selected a usb device.");

            if (usbDevices.SelectedIndex < 0)
            {
                Log.Error("Selected USB Device index was less than 0.");
                applyUSB.IsEnabled = false;
                usbName.IsEnabled = false;
            }
            else
            {
                applyUSB.IsEnabled = true;
                usbName.IsEnabled = true;
                usbName.Text = USBDevices[usbDevices.SelectedIndex].DeviceName;
                Log.Information("Selected USB device " + USBDevices[usbDevices.SelectedIndex].DeviceName);
            }
        }
        private void SelectedDeviceBT(object sender, SelectionChangedEventArgs e)
        {
            Log.Information("Selected a bluetooth device.");
            if (BtDevices.SelectedIndex < 0)
            {
                Log.Error("Selected bluetooth device index was less than 0.");
                btName.IsEnabled = false;
                applyBT.IsEnabled = false;
            }
            else
            {
                btName.IsEnabled = true;
                applyBT.IsEnabled = true;
                btName.Text = BluetoothDevices[BtDevices.SelectedIndex].DeviceName;
                Log.Information("Selected bluetooth device" + BluetoothDevices[BtDevices.SelectedIndex].DeviceName);
            }
        }

        private async void RefreshBT(object sender, RoutedEventArgs e)
        {
            BtDevices.Items.Clear();
            BluetoothDevices.Clear();
            Log.Information("Refreshing bluetooth devices.");
            try
            {
                DeviceInformationCollection PairedDevices = await DeviceInformation.FindAllAsync(BluetoothDevice.GetDeviceSelectorFromPairingState(true));
                Log.Information($"Found {PairedDevices.Count} bluetooth devices.");
                foreach (var i in PairedDevices)
                {
                    BluetoothDevices.Add(new BluetoothDevices { DeviceID = i.Id, DeviceName = i.Name, DeviceType = DeviceTypes.Bluetooth });
                    BtDevices.Items.Add($"{i.Name} ({i.Id})");
                    Log.Information($"Adding bluetooth device {i.Name} ({i.Id}) into the list.");
                }
                btName.Text = "";

                if (PairedDevices.Count < 1)
                {
                    Log.Error("Unable to find any bluetooth devices.");
                    Helper.Error("Error", "Unable to find any bluetooth devices. Make sure you have paired your devices with this computer before and try again.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error scanning for bluetooth devices.");
                Helper.Error("Error", "There was a unknown error scanning for bluetooth devices. Contact the developer");
            }
        }

        private void RefreshUSB(object sender, RoutedEventArgs e)
        {
            usbDevices.Items.Clear();
            Log.Information("Refreshing USB devices.");
            try
            {
                ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_USBHub");

                Log.Information($"Found {mos.Get().Count} USB devices.");

                foreach (ManagementObject usb in mos.Get())
                {
                    USBDevices.Add(new USBDevice { DeviceID = usb.Properties["DeviceID"].Value.ToString(), DeviceName = usb.Properties["Description"].Value.ToString(), DeviceType = DeviceTypes.USB });
                    usbDevices.Items.Add($"{usb.Properties["Description"].Value} ({usb.Properties["DeviceID"].Value})");
                    Log.Information($"Adding USB device {usb.Properties["Description"].Value} ({usb.Properties["DeviceID"].Value}).");
                }
                usbName.Text = "";
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error scanning for USB devices.");
                Helper.Error("Error", "There was a error scanning for USB devices. You might want to contact the developer.");
            }
        }

        private void ApplyBT(object sender, RoutedEventArgs e)
        {
            Log.Information("Applying selected bluetooth device.");
            try
            {
                BluetoothDevices SelectedDevice = BluetoothDevices[BtDevices.SelectedIndex];
                PairedDevicesJson PairedDevices = PairedDevicesJson.FromJson(File.ReadAllText(Helper.PairedDevicesFile));
                Log.Information("Got current devices.");
                PairedDevices.Devices.Add(new Device { DeviceAddress = SelectedDevice.DeviceID, DeviceName = btName.Text, DeviceType = "Bluetooth", TemplateLocation = "" });
                File.WriteAllText(Helper.PairedDevicesFile, PDSerialize.ToJson(PairedDevices));
                Log.Information("Wrote to PairedDevices.json");

                MessageBox.Show("Done!", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "There was a error applying the bluetooth device.");
                Helper.Error("Error", "There was a error writing to the PairedDevices.json file. Please contact the developer.");
            }
        }

        private void ApplyUSB(object sender, RoutedEventArgs e)
        {
            Log.Information("Applying selected USB device.");
            try
            {
                USBDevice SelectedDevice = USBDevices[usbDevices.SelectedIndex];
                PairedDevicesJson PairedDevices = PairedDevicesJson.FromJson(File.ReadAllText(Helper.PairedDevicesFile));
                Log.Information("Got current devices.");
                PairedDevices.Devices.Add(new Device { DeviceAddress = SelectedDevice.DeviceID, DeviceName = usbName.Text, DeviceType = "USB", TemplateLocation = "" });
                File.WriteAllText(Helper.PairedDevicesFile, PDSerialize.ToJson(PairedDevices));
                Log.Information("Wrote to PairedDevices.json");
                MessageBox.Show("Done!", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "There was a error applying the USB device.");
                Helper.Error("Error", "There was a error writing to the PairedDevices.json file. Please contact the developer.");
            }
        }

        private void Page_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Log.Information("Scrolled.");
            try
            {
                Log.Information("Current delta is " + e.Delta + " and current vertical offset is " + mainScrollBar.VerticalOffset + ".");
                if (e.Delta < 0)
                {
                    mainScrollBar.ScrollToVerticalOffset(mainScrollBar.VerticalOffset + 30);
                }
                else if (e.Delta > 0)
                {
                    mainScrollBar.ScrollToVerticalOffset(mainScrollBar.VerticalOffset - 30);
                }
                Log.Information("New vertical offset is " + mainScrollBar.VerticalOffset);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unable to scroll.");
            }
        }
    }
}
