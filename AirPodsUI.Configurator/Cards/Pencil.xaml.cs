using AirPodsUI.Configurator.Configuration;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

namespace AirPodsUI.Configurator.Cards
{
    /// <summary>
    /// Interaction logic for PencilPopup.xaml
    /// </summary>
    public partial class Pencil : Window
    {
        Timer timer;
        bool FadingBottom;

        public Pencil(PencilConfig config)
        {
            // Set variables
            InitializeComponent();
            timer = new Timer();

            // Set colors and text
            background.Background = config.Background.ToBrush();
            devIcon.Source = new BitmapImage(new Uri(config.IconLocation, UriKind.RelativeOrAbsolute));
            devName.Content = config.StaticName;
            devName.Foreground = config.DeviceNameForeground.ToBrush();
            devStatus.Content = config.StatusText;
            devStatus.Foreground = config.StatusForeground.ToBrush();

            // Set timer to close after 5 seconds
            timer.Interval = 5000;
            timer.Elapsed += Timer_Elapsed;

            this.FadingBottom = false;

            // Set to middle of screen
            this.Left = (SystemParameters.WorkArea.Width / 2) - (this.Width / 2);
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Fade and Close
            timer.Stop();
            if (!FadingBottom)
                Dispatcher.Invoke(() => Fade((int)this.Top, -60, -10, 10, true, true));
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Fade and start timer
            if (!FadingBottom)
                Dispatcher.Invoke(() => Fade(-60, 20, 10, 10, false, false));
            timer.Start();
        }
    }
}
