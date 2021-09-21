using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AirPodsUI.Core.Models;

namespace AirPodsUI.Settings.Pages
{
    /// <summary>
    /// Interaction logic for DeviceSettings.xaml
    /// </summary>
    public partial class DeviceSettings : Page
    {
        public static string ID { get; set; }

        private Device selectedDevice;

        public DeviceSettings()
        {
            InitializeComponent();
        }

        private async void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                selectedDevice = App.Devices.Where((i) => i.Identifier == ID).FirstOrDefault();

                sNameTitle.Content = selectedDevice.Name;
                sName.Text = selectedDevice.Name;
                sID.Text = selectedDevice.Identifier;
                sDarkMode.IsChecked = selectedDevice.DarkMode;
                sToastType.SelectedIndex = 0;

                using (Core.Settings settings = new Core.Settings())
                {
                    if (settings.AllowIDEditing)
                    {
                        sID.IsEnabled = true;
                        sID.IsReadOnly = false;
                    }
                }
            }
            catch (Exception)
            {
                await Dialog.ShowDialogAsync("Error", "An unknown error occured trying to retrieve the device details.", "OK");
            }
        }

        private async void OnApplyClicked(object sender, RoutedEventArgs e)
        {
            selectedDevice.Name = sName.Text;
            selectedDevice.Identifier = sID.Text;

            int index = App.Devices.FindIndex((e) =>
            {
                return e == selectedDevice;
            });

            if (index < 0)
            {
                await Dialog.ShowDialogAsync("Error", "Unable to save changes!", "OK");
            }
            else
            {
                if (sToastType.SelectedIndex != 0)
                {
                    await Dialog.ShowDialogAsync("Notice", "More toast types are coming soon!", "Great!");
                    return;
                }

                App.Devices[index].Name = sName.Text;
                sNameTitle.Content = sName.Text;
                App.Devices[index].Identifier = sID.Text;
                App.Devices[index].DarkMode = sDarkMode.IsChecked.GetValueOrDefault(false);
                DevicesJson.SaveDevices(App.Devices);
                App.InvokeDeviceChange(this);
                await Dialog.ShowDialogAsync("Success", "Successfully applied changed to device!", "OK");
            }
        }

        private void OnDeleteClicked(object sender, RoutedEventArgs e)
        {
            sDeleteFlyout.Hide();

            App.Devices.Remove(selectedDevice);
            DevicesJson.SaveDevices(App.Devices);

            App.InvokeDeviceChange(this);
        }
    }
}
