using System.Windows;
using AirPodsUI.Settings.Pages;

namespace AirPodsUI.Settings.Windows
{
    /// <summary>
    /// Interaction logic for DeviceWizard.xaml
    /// </summary>
    public partial class DeviceWizard : Window
    {
        public DeviceWizard()
        {
            InitializeComponent();
            sFrame.NavigationService.Navigate(new WizardIntro());
        }
    }
}
