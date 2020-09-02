using AirPodsUI.Configurator.Cards;
using AirPodsUI.Configurator.Configuration;
using Microsoft.Win32;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
            Log.Information("Loading window with " + json.Devices[selectedIndex].DeviceName);
            InitializeComponent();
            Configuration = new Dictionary<string, object>();
            this.json = json;
            currentDev = selectedIndex;
            refresh_Click(this, new RoutedEventArgs());
            DevName.Content = json.Devices[selectedIndex].DeviceName + " settings";
            Log.Information("Loaded window.");
        }

        private void remove_Click(object sender, RoutedEventArgs e)
        {
            Log.Information("Removing device.");
            try
            {
                File.Delete(Configuration.Keys.ToArray()[Templates.SelectedIndex]);
                refresh_Click(sender, e);
            }
            catch (Exception el)
            {
                Log.Error(el, "Error removing the file!");
                Helper.Error("Error", "Unable to remove file.");
            }
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "AirPodsUI Templates (*.card, *.pencil, *.notif)|*.card;*.pencil;*.notif";
                if (ofd.ShowDialog() == true)
                {
                    File.Copy(ofd.FileName, Helper.NextAvailableFilename(Helper.TemplateFolder + $"\\{System.IO.Path.GetFileName(ofd.FileName)}"));
                    refresh_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error has occurred adding template.");
                Helper.Error("Error", "Unable to add template.");
            }
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            try
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
                        Templates.Items.Add($"(Card) { ((CardConfig)Configuration[files[i]]).TemplateName }");
                    }
                    else if (ext == ".pencil")
                    {
                        Configuration.Add(files[i], ConfigParser.ParseP(files[i]));
                        Templates.Items.Add($"(Pencil) { ((PencilConfig)Configuration[files[i]]).TemplateName }");
                    }
                    else if (ext == ".notif")
                    {
                        Configuration.Add(files[i], ConfigParser.ParseN(files[i]));
                        Templates.Items.Add($"(Banner) { ((NotificationConfig)Configuration[files[i]]).TemplateName }");
                    }

                    if (json.Devices[currentDev].TemplateLocation == files[i])
                    {
                        Templates.SelectedIndex = i;
                    }
                }
            }
            catch (Exception)
            {
                Helper.Error("Error", "Unable to refresh templates list.");
            }
        }

        private void dupe_Click(object sender, RoutedEventArgs e)
        {
            if (Templates.SelectedIndex < 0)
            {
                Helper.Error("Error", "Please select a template from the list in order to continue.");
                return;
            }

            try
            {
                File.Copy(Configuration.Keys.ToArray()[Templates.SelectedIndex], Helper.NextAvailableFilename(Configuration.Keys.ToArray()[Templates.SelectedIndex]));
                refresh_Click(sender, e);
            }
            catch (Exception)
            {
                Helper.Error("Error", "Unable to duplicate template.");
            }
        }

        private void tempConfig_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Coming soon!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void applyConfig_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                json.Devices[currentDev].TemplateLocation = Configuration.Keys.ToArray()[Templates.SelectedIndex];
                File.WriteAllText(Helper.PairedDevicesFile, PDSerialize.ToJson(json));
                MessageBox.Show("Applied!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception)
            {
                Helper.Error("Error", "Unable to apply configuration.");
            }
        }

        private void remDev_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to remove this device?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    json.Devices.RemoveAt(currentDev);
                    File.WriteAllText(Helper.PairedDevicesFile, PDSerialize.ToJson(json));
                    MessageBox.Show("Removed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception)
                {
                    Helper.Error("Error", "Unable to remove device.");
                }
            }
        }

        private void newCard_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CardConfig config = new CardConfig();
                File.WriteAllText(Helper.NextAvailableFilename(Helper.TemplateFolder + $"\\{config.TemplateName}.card"), CreateConfigFile.Create(config));
                refresh_Click(sender, e);
            }
            catch (Exception)
            {
                Helper.Error("Error", "Unable to create new template.");
            }
        }

        private void newBanner_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NotificationConfig config = new NotificationConfig();
                File.WriteAllText(Helper.NextAvailableFilename(Helper.TemplateFolder + $"\\{config.TemplateName}.notif"), CreateConfigFile.Create(config));
                refresh_Click(sender, e);
            }
            catch (Exception)
            {
                Helper.Error("Error", "Unable to create new template.");
            }
        }

        private void newPencil_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PencilConfig config = new PencilConfig();
                File.WriteAllText(Helper.NextAvailableFilename(Helper.TemplateFolder + $"\\{config.TemplateName}.pencil"), CreateConfigFile.Create(config));
                refresh_Click(sender, e);
            }
            catch (Exception)
            {
                Helper.Error("Error", "Unable to create new template.");
            }
        }

        private void ShowPreview(PencilConfig config)
        {
            // Set colors and text
            pencilBack.Background = config.Background.ToBrush();
            pDevIcon.Source = new BitmapImage(new Uri(config.IconLocation, UriKind.RelativeOrAbsolute));
            pDevName.Content = config.StaticName;
            pDevName.Foreground = config.DeviceNameTextForeground.ToBrush();
            pDevStatus.Content = config.StatusText;
            pDevStatus.Foreground = config.StatusTextForeground.ToBrush();

            pencilBack.Visibility = Visibility.Visible;
            cTint.Visibility = Visibility.Hidden;
            banner.Visibility = Visibility.Hidden;
        }

        private void ShowPreview(NotificationConfig config)
        {
            bBack.Background = config.Background.ToBrush();
            bIcon.Source = new BitmapImage(new Uri(config.IconLocation));
            bAppName.Content = config.AppName;
            bAppName.Foreground = config.AppNameColor.ToBrush();
            bAppDate.Foreground = config.AppNameColor.ToBrush();
            bCaption.Content = config.StaticName;
            bCaption.Foreground = config.CaptionForeground.ToBrush();
            bStatus.Content = config.StatusText;
            bStatus.Foreground = config.StatusTextForeground.ToBrush();

            pencilBack.Visibility = Visibility.Hidden;
            cTint.Visibility = Visibility.Hidden;
            banner.Visibility = Visibility.Visible;
        }

        private void ShowPreview(CardConfig config)
        {
            bool loop = bool.Parse(config.Loop);

            // Try for image
            bool isImage = false;

            string ext = System.IO.Path.GetExtension(config.MediaLocation);
            if (config.MediaLocation.StartsWith("pack://application:,,,/Assets/"))
            {
                Stream stream = Application.GetResourceStream(new Uri(config.MediaLocation)).Stream;

                Directory.CreateDirectory(System.IO.Path.GetTempPath() + "\\AirPodsUI\\Assets\\");

                using (FileStream file = new FileStream(System.IO.Path.GetTempPath() + "\\AirPodsUI\\Assets\\file" + ext, FileMode.Create))
                {
                    stream.CopyTo(file);
                }

                config.MediaLocation = System.IO.Path.GetTempPath() + "\\AirPodsUI\\Assets\\file" + ext;
            }

            try
            {
                System.Drawing.Image.FromFile(config.MediaLocation);
                isImage = true;
            }
            catch (OutOfMemoryException)
            {
                isImage = false;
            }

            if (config.StretchMode == "None")
            {
                cImage.Stretch = Stretch.None;
                cMedia.Stretch = Stretch.None;
            }
            if (config.StretchMode == "Fill")
            {
                cImage.Stretch = Stretch.Fill;
                cMedia.Stretch = Stretch.Fill;
            }
            if (config.StretchMode == "UniformToFill")
            {
                cImage.Stretch = Stretch.UniformToFill;
                cMedia.Stretch = Stretch.UniformToFill;
            }
            if (config.StretchMode == "Uniform")
            {
                cImage.Stretch = Stretch.Uniform;
                cMedia.Stretch = Stretch.Uniform;
            }

            if (isImage)
            {
                cImage.Source = new BitmapImage(new Uri(config.MediaLocation));
                cImage.Visibility = Visibility.Visible;
            }
            else
            {
                cMedia.Source = new Uri(config.MediaLocation);
                cMedia.Play();
                cMedia.Visibility = Visibility.Visible;
                if (loop)
                    cMedia.MediaEnded += Media_MediaEnded;
            }

            if (config.Tint != "")
                cTint.Background = config.Tint.ToBrush();

            cBack.Background = config.Background.ToBrush();
            cDevName.Content = config.StaticName;
            cDevName.Foreground = config.NameForeground.ToBrush();
            cMedia.Source = new Uri(config.MediaLocation);
            cDoneBtn.Content = config.ButtonText;
            cDoneBtn.Background = config.ButtonBackground.ToBrush();
            cDoneBtn.Foreground = config.ButtonForeground.ToBrush();

            pencilBack.Visibility = Visibility.Hidden;
            cTint.Visibility = Visibility.Visible;
            banner.Visibility = Visibility.Hidden;
        }

        private void Media_MediaEnded(object sender, RoutedEventArgs e)
        {
            cMedia.Position = TimeSpan.Zero;
            cMedia.Play();
        }

        private void Templates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cMedia.Source = null;
            if (Templates.SelectedIndex < 0)
                return;

            this.Dispatcher.Invoke(() =>
            {
                switch (System.IO.Path.GetExtension(Configuration.Keys.ToArray()[Templates.SelectedIndex]).ToLower())
                {
                    case ".pencil":
                        ShowPreview((PencilConfig)Configuration[Configuration.Keys.ToArray()[Templates.SelectedIndex]]);
                        break;
                    case ".notif":
                        ShowPreview((NotificationConfig)Configuration[Configuration.Keys.ToArray()[Templates.SelectedIndex]]);
                        break;
                    case ".card":
                        ShowPreview((CardConfig)Configuration[Configuration.Keys.ToArray()[Templates.SelectedIndex]]);
                        break;
                    default:
                        pencilBack.Visibility = Visibility.Hidden;
                        cTint.Visibility = Visibility.Hidden;
                        banner.Visibility = Visibility.Hidden;
                        break;
                }
            });
        }
    }
}
