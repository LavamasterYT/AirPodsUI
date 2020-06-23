using AdonisUI;
using System.Windows;
using Microsoft.Win32;
using AdonisUI.Controls;
using AirPodsUI.Configurator.Pages;

namespace AirPodsUI.Configurator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : AdonisWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            // Set the page to the read me 
            MainFrame.NavigationService.Navigate(new MainPage());

            // Get system app theme and set app theme based on values
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            if (int.TryParse(reg.GetValue("AppsUseLightTheme").ToString(), out int result))
            {
                if (result == 0)
                {
                    TitleBarBackground = System.Windows.Media.Brushes.Black;
                    ResourceLocator.SetColorScheme(Application.Current.Resources, ResourceLocator.DarkColorScheme);
                }
                else
                {
                    TitleBarBackground = System.Windows.Media.Brushes.White;
                    ResourceLocator.SetColorScheme(Application.Current.Resources, ResourceLocator.LightColorScheme);
                }
            }
        }

        private void ShowPairPage(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new PairPage());
        }
    }
}