using System;
using System.Windows;
using AirPodsUI.Core;
using System.Diagnostics;
using System.Windows.Controls;

namespace AirPodsUI.Settings.Pages
{
    public partial class TraySettings : Page
    {
        Core.Settings settings;

        public TraySettings()
        {
            InitializeComponent();
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Logger.Log("Retrieving settings from the registry");

                settings = new Core.Settings();

                sRefreshRate.Text = settings.RefreshRate.ToString();
                sRunAtStartup.IsOn = settings.RunAtStartup;
                sAllowEditing.IsOn = settings.AllowIDEditing;
                sOffset.Text = settings.Offset.ToString();

                settings.Dispose();
            }
            catch (Exception ex)
            {
                Logger.Log("Unable to retrieve settings", ex);
            }
        }

        private async void OnApplyClicked(object sender, RoutedEventArgs e)
        {
            settings = new Core.Settings();

            try
            {
                settings.RefreshRate = int.Parse(sRefreshRate.Text);
                settings.RunAtStartup = sRunAtStartup.IsOn;
                settings.AllowIDEditing = sAllowEditing.IsOn;
                settings.Offset = int.Parse(sOffset.Text);

                Logger.Log("Successfully saved the changes to the registry");
                await Dialog.ShowDialogAsync("Saved", "Changed have been saved", "OK");
            }
            catch (FormatException)
            {
                await Dialog.ShowDialogAsync("Error", "Please input numbers only", "OK");
            }
            catch (Exception ex)
            {
                Logger.Log("Unable to save the changes to the registry", ex);
                await Dialog.ShowDialogAsync("Error", "Unable to save the changes to the registry", "OK");
            }

            settings.Dispose();
        }

        private void OnRestartClicked(object sender, RoutedEventArgs e)
        {
            Logger.Log("Restarting the tray service");

            try
            {
                Process[] p = Process.GetProcessesByName("AirPodsUI.Tray");

                Logger.Log($"Found {p.Length} processes");

                foreach (var i in p)
                {
                    Logger.Log($"Killing {i.Id}");
                    i.Kill();
                }

                Logger.Log("Starting process");

                Process.Start($"{AppDomain.CurrentDomain.BaseDirectory}\\AirPodsUI.Tray.exe");
            }
            catch (Exception ex)
            {
                Logger.Log("Unable to restart the tray service", ex);
            }
        }
    }
}
