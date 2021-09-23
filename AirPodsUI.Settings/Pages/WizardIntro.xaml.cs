using System.Windows;
using System.Windows.Controls;

namespace AirPodsUI.Settings.Pages
{
    public partial class WizardIntro : Page
    {
        public WizardIntro()
        {
            InitializeComponent();
        }

        private void OnNextClicked(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new WizardPair());
        }
    }
}
