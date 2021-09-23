using System;
using System.Linq;
using AirPodsUI.Core;
using System.Windows;
using ModernWpf.Controls;
using AirPodsUI.Core.Models;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Collections.Generic;
using AirPodsUI.Settings.Windows;

namespace AirPodsUI.Settings.Pages
{
    public partial class AddDevice : System.Windows.Controls.Page
    {
        List<PnPDevice> devices;
        List<PnPDevice> listDevices;

        public AddDevice()
        {
            devices = new List<PnPDevice>();
            listDevices = new List<PnPDevice>();

            InitializeComponent();
        }

        private async void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            await Refresh(true);
        }

        private async Task Refresh(bool refreshUsb)
        {
            if (refreshUsb)
            {
                Logger.Log(LogType.Information, "Refreshing PNP devices");
                await RefreshDevices();
            }

            Logger.Log(LogType.Information, "Refreshing device view");

            sDevices.ItemsSource = listDevices;
            sDevices.Items.Refresh();
        }

        private async Task RefreshDevices()
        {
            await Task.Run(() =>
            {
                try
                {
                    devices = PNP.GetPNPDevices();
                    listDevices = new List<PnPDevice>(devices);
                }
                catch (Exception e)
                {
                    listDevices = new List<PnPDevice>();
                    Logger.Log(LogType.Warning, "Unable to scan for devices!", e);
                }
            });
        }

        private async void ShowAllChecked(object sender, RoutedEventArgs e)
        {
            await Refresh(true);
        }

        private void OnDeviceSelected(object sender, SelectionChangedEventArgs e)
        {
            if (sDevices.SelectedIndex >= 0)
            {
                Logger.Log(LogType.Information, "Selected device from list");

                sDeviceData.IsEnabled = true;
                sName.Text = ((PnPDevice)sDevices.SelectedItem).Name;
                sID.Text = ((PnPDevice)sDevices.SelectedItem).PNPDeviceID;
            }
            else
            {
                sDeviceData.IsEnabled = false;
            }
        }

        private async void OnAddClick(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var i in App.Devices)
                {
                    if (i.Identifier == sID.Text)
                    {
                        Logger.Log(LogType.Warning, $"Unable to add device \"{i.Name}\" with ID \"{i.Identifier}\" because it already exists");
                        await Dialog.ShowDialogAsync("Warning", $"A device with the name \"{i.Name}\" already exists with the same ID!", "OK");

                        return;
                    }
                }
                
                ContentDialogResult result = await Dialog.ShowDialogAsync(
                    "Add Device", 
                    $"Are you sure you want to add the following device?\n\nName: {sName.Text}\n\nID: {sID.Text}", 
                    "No", 
                    "Yes");

                if (result == ContentDialogResult.Primary)
                {
                    try
                    {
                        Logger.Log(LogType.Information, $"Attempting to add device \"{sName.Text}\" with ID \"{sID.Text}\"");
                        Device dev = new Device
                        {
                            DarkMode = true,
                            Identifier = sID.Text,
                            Name = sName.Text,
                            ToastType = Toast.Pencil
                        };

                        App.Devices.Add(dev);
                        DevicesJson.SaveDevices(App.Devices);

                        App.InvokeDeviceChange(this);

                        Logger.Log(LogType.Information, $"Successfully saved device \"{sName.Text}\" with ID \"{sID.Text}\"");
                        await Dialog.ShowDialogAsync("Success", $"Device \"{sName.Text}\" added successfully!", "OK");
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(LogType.Warning, $"Was unable to add device \"{sName.Text}\" with ID \"{sID.Text}\"", ex);
                        await Dialog.ShowDialogAsync("Error", $"Unable to add device \"{sName.Text}\"", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LogType.Warning, $"Was unable to add device \"{sName.Text}\" with ID \"{sID.Text}\" because an unknown error occured", ex);
                await Dialog.ShowDialogAsync("Error", "An unknown error occured trying to add the device", "OK");
            }
        }

        private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                listDevices = devices.Where((i) =>
                {
                    try
                    {
                        return i.Name.ToLower().Contains(sSearchField.Text.ToLower());
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }).ToList();

                await Refresh(false);
            }
            catch (Exception ex)
            {
                Logger.Log(LogType.Warning, "Unable to search devices for some reason", ex);
            }
        }

        private async void OnFilterChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Logger.Log(LogType.Information, "Filtering devices");

                listDevices = devices.Where((i) =>
                {
                    try
                    {
                        ComboBoxItem item = sFilter.SelectedItem as ComboBoxItem;

                        string[] filterText = item.Content.ToString().Split('/');

                        foreach (var j in filterText)
                        {
                            return i.PNPDeviceID.ToLower().StartsWith(j.ToLower());
                        }

                        return false;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }).ToList();

                await Refresh(false);
            }
            catch (Exception ex)
            {
                Logger.Log(LogType.Warning, "Unable to filter devices for some reason", ex);
            }
        }

        private void OnOpenWizardClick(object sender, RoutedEventArgs e)
        {
            DeviceWizard wizard = new DeviceWizard();
            wizard.ShowDialog();
        }
    }
}
