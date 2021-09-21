using AirPodsUI.Core;
using AirPodsUI.Core.Models;
using ModernWpf.Controls;
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

namespace AirPodsUI.Settings.Pages
{
    /// <summary>
    /// Interaction logic for AddUSB.xaml
    /// </summary>
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
                await RefreshDevices();

            sDevices.ItemsSource = listDevices;
            sDevices.Items.Refresh();
        }

        private async Task RefreshDevices()
        {
            await Task.Run(() =>
            {
                devices = PNP.GetPNPDevices();
                listDevices = new List<PnPDevice>(devices);
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

                    await Dialog.ShowDialogAsync("Success", $"Device \"{sName.Text}\" added successfully!", "OK");
                }
            }
            catch (Exception)
            {
                await Dialog.ShowDialogAsync("Error", "Unable to add device", "OK");
            }
        }

        private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
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

        private async void OnFilterChanged(object sender, SelectionChangedEventArgs e)
        {
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
    }
}
