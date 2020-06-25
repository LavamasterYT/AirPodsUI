using AirPodsUI.Configurator.Cards;
using AirPodsUI.Configurator.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

namespace AirPodsUI.Configurator.Pages
{
    /// <summary>
    /// Interaction logic for DeviceConfigPage.xaml
    /// </summary>
    public partial class DeviceConfigPage : Page
    {
        public Dictionary<string, object> Configuration;
        public PairedDevicesJson json;
        public int currentDev;

        public DeviceConfigPage(PairedDevicesJson json, int selectedIndex)
        {
            InitializeComponent();
            Configuration = new Dictionary<string, object>();
            this.json = json;
            currentDev = selectedIndex;
            refresh_Click(this, new RoutedEventArgs());
            DevName.Content = json.Devices[selectedIndex].DeviceName + " settings";
        }

        private void remove_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {

        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            Templates.Items.Clear();
            Configuration.Clear();
            string[] files = Directory.GetFiles(Helper.TemplateFolder);
            for (int i = 0; i < files.Length; i++)
            {
                string ext = System.IO.Path.GetExtension(files[i]).ToLower();
                if (ext == ".card")
                {
                    Configuration.Add(files[i], ConfigParser.ParseC(files[i]));
                    Templates.Items.Add($"(Card) { ((CardConfig)Configuration[files[i]]).Name }");
                }
                else if (ext == ".pencil")
                {
                    Configuration.Add(files[i], ConfigParser.ParseP(files[i]));
                    Templates.Items.Add($"(Pencil) { ((PencilConfig)Configuration[files[i]]).Name }");
                }
                else if (ext == ".notif")
                {
                    Configuration.Add(files[i], ConfigParser.ParseN(files[i]));
                    Templates.Items.Add($"(Banner) { ((NotificationConfig)Configuration[files[i]]).Name }");
                }

                if (json.Devices[currentDev].TemplateLocation == files[i])
                {
                    Templates.SelectedIndex = i;
                }
            }
        }

        private void dupe_Click(object sender, RoutedEventArgs e)
        {

        }

        private void new_Click(object sender, RoutedEventArgs e)
        {

        }

        private void tempConfig_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Coming soon!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void applyConfig_Click(object sender, RoutedEventArgs e)
        {
            json.Devices[currentDev].TemplateLocation = Configuration.Keys.ToArray()[Templates.SelectedIndex];
            File.WriteAllText(Helper.PairedDevicesFile, PDSerialize.ToJson(json));
            MessageBox.Show("Applied!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void remDev_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to remove this device?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                json.Devices.RemoveAt(currentDev);
                File.WriteAllText(Helper.PairedDevicesFile, PDSerialize.ToJson(json));
            }
        }
    }
}
