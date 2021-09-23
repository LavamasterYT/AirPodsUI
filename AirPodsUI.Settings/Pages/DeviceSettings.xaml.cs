using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AirPodsUI.Core.Models;
using AirPodsUI.Core;

namespace AirPodsUI.Settings.Pages
{
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

                Logger.Log(LogType.Information, "Retrieving device details for " + selectedDevice.Name);

                sNameTitle.Content = selectedDevice.Name;
                sName.Text = selectedDevice.Name;
                sID.Text = selectedDevice.Identifier;
                sDarkMode.IsOn = selectedDevice.DarkMode;
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
            catch (Exception ex)
            {
                Logger.Log(LogType.Warning, "An unknown error occured trying to retrieve the device details.", ex);
                await Dialog.ShowDialogAsync("Error", "An unknown error occured trying to retrieve the device details.", "OK");
            }
        }

        private async void OnApplyClicked(object sender, RoutedEventArgs e)
        {
            selectedDevice.Name = sName.Text;
            selectedDevice.Identifier = sID.Text;

            Logger.Log(LogType.Information, "Saving device information");

            try
            {
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
                    App.Devices[index].DarkMode = sDarkMode.IsOn;
                    DevicesJson.SaveDevices(App.Devices);
                    App.InvokeDeviceChange(this);
                    await Dialog.ShowDialogAsync("Success", "Successfully applied changed to device!", "OK");
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Unable to save device changes", ex);
            }
        }

        private void OnDeleteClicked(object sender, RoutedEventArgs e)
        {
            Logger.Log("Deleting current device");
            
            try
            {
                sDeleteFlyout.Hide();

                App.Devices.Remove(selectedDevice);
                DevicesJson.SaveDevices(App.Devices);

                App.InvokeDeviceChange(this);

                this.NavigationService.Navigate(null);
            }
            catch (Exception ex)
            {
                Logger.Log("Unable to delete device/an error occured deleting the device", ex);
            }
        }
    }
}
