using AirPodsUI.Configurator.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Windows.Media.Streaming.Adaptive;

namespace AirPodsUI.Configurator.Cards
{
    /// <summary>
    /// Interaction logic for BannerPopup.xaml
    /// </summary>
    public partial class Banner : Window
    {
        Timer timer;
        MediaPlayer player;
        IntPtr focused;
        public Banner(NotificationConfig config)
        {
            InitializeComponent();
            Loaded += BannerPopup_Loaded;
            focused = Helper.ActiveWindowHandle;

            // Setting variables
            timer = new Timer();
            player = new MediaPlayer();

            // Setting notification
            if (config.NotifSound.StartsWith("pack://application:,,,/Assets/"))
            {
                // If its a preset, write to a file to play
                Stream stream = Application.GetResourceStream(new Uri(config.NotifSound)).Stream;

                Directory.CreateDirectory(System.IO.Path.GetTempPath() + "\\AirPodsUI\\Assets\\");

                using (FileStream file = new FileStream(System.IO.Path.GetTempPath() + "\\AirPodsUI\\Assets\\sound.mp3", FileMode.Create))
                {
                    stream.CopyTo(file);
                }

                player.Open(new Uri(System.IO.Path.GetTempPath() + "AirPodsUI\\Assets\\sound.mp3", UriKind.RelativeOrAbsolute));
            }
            else
            {
                player.Open(new Uri(config.NotifSound, UriKind.RelativeOrAbsolute));
            }

            background.Background = config.Background.ToBrush();
            icon.Source = new BitmapImage(new Uri(config.IconPath));
            appName.Content = config.AppName;
            appName.Foreground = config.AppNameForeground.ToBrush();
            appDate.Foreground = config.AppNameForeground.ToBrush();
            caption.Content = config.StaticName;
            caption.Foreground = config.CaptionForeground.ToBrush();
            status.Content = config.StatusText;
            status.Foreground = config.StatusForeground.ToBrush();

            // Timer to close after 3.5 seconds
            timer.Interval = 3500;
            timer.Elapsed += Timer_Elapsed;

            // Set to middle of the screen
            this.Left = (SystemParameters.WorkArea.Width / 2) - (this.Width / 2);
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Delete temp dir if exists and exit
            timer.Stop();
            if (Directory.Exists(System.IO.Path.GetTempPath() + "\\AirPodsUI"))
                Directory.Delete(System.IO.Path.GetTempPath() + "\\AirPodsUI", true);
            Dispatcher.Invoke(() => Fade((int)this.Top, -90, -10, 10, true, true));
        }

        private void BannerPopup_Loaded(object sender, RoutedEventArgs e)
        {
            Helper.SetActiveWindow(focused);
            // Play sound and fade and start timer
            player.Play();
            Dispatcher.Invoke(() => Fade(-90, 10, 10, 10, false, false));
            timer.Start();
        }

        private async void Fade(int start, int less, int add, int interval, bool close, bool greaterThan)
        {
            if (!greaterThan)
            {
                for (int i = start; i <= less; i += add)
                {
                    this.Top = i;
                    await Task.Delay(interval);
                }
            }
            else
            {
                for (int i = start; i >= less; i += add)
                {
                    this.Top = i;
                    await Task.Delay(interval);
                }
            }
            if (close) this.Close();
        }
    }
}
