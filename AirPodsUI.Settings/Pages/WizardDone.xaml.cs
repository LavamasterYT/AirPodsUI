using System;
using System.Windows;
using AirPodsUI.Core;
using AirPodsUI.Core.Models;
using System.Windows.Controls;

namespace AirPodsUI.Settings.Pages
{
    public partial class WizardDone : Page
    {
        public WizardDone(PnPDevice device)
        {
            InitializeComponent();

            AirPodsUI.Core.Settings settings = new AirPodsUI.Core.Settings();

            if (settings.AllowIDEditing)
            {
                sID.IsReadOnly = false;
                sID.IsEnabled = true;
            }

            sID.Text = device.PNPDeviceID;
            sName.Text = device.Name;
        }

        private void OnBackClicked(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new WizardPair());
        }

        private void OnDoneClicked(object sender, RoutedEventArgs e)
        {
            try
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
            }
            catch (Exception ex)
            {
                Logger.Log("Unable to save the device from the pairing wizard", ex);
            }

            Window.GetWindow(this).Close();
        }
    }
}
