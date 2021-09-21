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

namespace AirPodsUI.Settings.Pages
{
    /// <summary>
    /// Interaction logic for TraySettings.xaml
    /// </summary>
    public partial class TraySettings : Page
    {
        Core.Settings settings;

        public TraySettings()
        {
            InitializeComponent();
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            settings = new Core.Settings();

            sRefreshRate.Text = settings.RefreshRate.ToString();
            sRunAtStartup.IsChecked = (bool)settings.RunAtStartup;
            sAllowEditing.IsChecked = (bool)settings.AllowIDEditing;
            sOffset.Text = settings.Offset.ToString();

            settings.Dispose();
        }

        private async void OnApplyClicked(object sender, RoutedEventArgs e)
        {
            settings = new Core.Settings();

            try
            {
                settings.RefreshRate = int.Parse(sRefreshRate.Text);
                settings.RunAtStartup = sRunAtStartup.IsChecked.GetValueOrDefault(false);
                settings.AllowIDEditing = sAllowEditing.IsChecked.GetValueOrDefault(false);
                settings.Offset = int.Parse(sOffset.Text);

                await Dialog.ShowDialogAsync("Saved", "Changed have been saved", "OK");
            }
            catch (FormatException)
            {

                await Dialog.ShowDialogAsync("Error", "Please input numbers only", "OK");
            }
            catch (Exception) { }

            settings.Dispose();
        }

        private void OnRestartClicked(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
