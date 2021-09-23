using System;
using System.Linq;
using AirPodsUI.Core;
using System.Windows;
using System.Threading;
using AirPodsUI.Core.Models;
using System.Windows.Controls;
using System.Collections.Generic;

namespace AirPodsUI.Settings.Pages
{
    public partial class WizardPair : Page
    {
        List<PnPDevice> lastTick;
        List<PnPDevice> curTick;
        List<PnPDevice> newDevices;

        bool busy = false;

        Timer tTimer;

        public WizardPair()
        {
            InitializeComponent();
            lastTick = new List<PnPDevice>();
            curTick = new List<PnPDevice>();
            newDevices = new List<PnPDevice>();
        }

        private async void OnNextClicked(object sender, RoutedEventArgs e)
        {
            foreach (var i in App.Devices)
            {
                if (i.Identifier == ((PnPDevice)sDevices.SelectedItem).PNPDeviceID)
                {
                    Logger.Log(LogType.Warning, "Tried adding a device that already exists");
                    await Dialog.ShowDialogAsync("Warning", $"A device with the name \"{i.Name}\" already exists with the same ID!", "OK");

                    return;
                }
            }

            this.NavigationService.Navigate(new WizardDone(((PnPDevice)sDevices.SelectedItem)));
        }

        private void OnBackClicked(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new WizardIntro());
        }

        private void OnTick(object s)
        {
            try
            {
                if (!busy)
                {
                    busy = true;

                    curTick = PNP.GetPNPDevices();

                    List<PnPDevice> diffs = curTick.Where((i) =>
                    {
                        return !(lastTick.Contains(i));
                    }).ToList();

                    foreach (var i in diffs)
                    {
                        if (!newDevices.Contains(i))
                        {
                            newDevices.Add(i);
                        }
                    }

                    this.Dispatcher.Invoke(() =>
                    {
                        PnPDevice selected = sDevices.SelectedItem as PnPDevice;

                        sDevices.ItemsSource = null;
                        sDevices.ItemsSource = newDevices;
                        sDevices.Items.Refresh();

                        if (selected != null)
                        {
                            if (sDevices.Items.Contains(selected))
                            {
                                sDevices.SelectedItem = selected;
                            }
                        }
                    });

                    busy = false;
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Unable to successfully scan for devices", ex);
                tTimer.Dispose();
            }
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            Logger.Log("Starting connection thread");
            tTimer = new Timer(OnTick, null, 1000, 500);

            lastTick = PNP.GetPNPDevices();
        }

        private void OnDeviceSelected(object sender, SelectionChangedEventArgs e)
        {
            if (sDevices.SelectedItem != null)
            {
                sNextBtn.IsEnabled = true;
            }
            else
            {
                sNextBtn.IsEnabled = false;
            }
        }

        private void OnPageUnloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                tTimer.Dispose();
            }
            catch (Exception ex)
            {
                Logger.Log("Timer was already disposed", ex);
            }
        }
    }
}
