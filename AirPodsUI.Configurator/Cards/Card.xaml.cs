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

namespace AirPodsUI.Configurator.Cards
{
    /// <summary>
    /// Interaction logic for Card.xaml
    /// </summary>
    public partial class Card : Window
    {
        bool bottomSpawn, usingTint, loop;
        Timer timer;
        IntPtr currentWin;
        Color tintBack;

        public Card(CardConfig config)
        {
            currentWin = Helper.ActiveWindowHandle;

            InitializeComponent();

            bottomSpawn = false;
            usingTint = true;
            timer = new Timer();
            timer.Interval = int.Parse(config.TimeOut);
            timer.Elapsed += Timer_Elapsed;

            loop = bool.Parse(config.Loop);

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
                image.Stretch = Stretch.None;
                media.Stretch = Stretch.None;
            }
            if (config.StretchMode == "Fill")
            {
                image.Stretch = Stretch.Fill;
                media.Stretch = Stretch.Fill;
            }
            if (config.StretchMode == "UniformToFill")
            {
                image.Stretch = Stretch.UniformToFill;
                media.Stretch = Stretch.UniformToFill;
            }
            if (config.StretchMode == "Uniform")
            {
                image.Stretch = Stretch.Uniform;
                media.Stretch = Stretch.Uniform;
            }

            if (isImage)
            {
                image.Source = new BitmapImage(new Uri(config.MediaLocation));
                image.Visibility = Visibility.Visible;
            }
            else
            {
                media.Source = new Uri(config.MediaLocation);
                media.Play();
                media.Visibility = Visibility.Visible;
                media.MediaEnded += Media_MediaEnded;
            }

            if (config.Location == "Bottom")
            {
                Opacity = 1;
                bottomSpawn = true;
            }
            else if (config.Location == "Center")
            {
                this.Left = SystemParameters.WorkArea.Width / 2 - 150;
            }

            if (config.Tint != "")
            {
                tintBack = (Color)ColorConverter.ConvertFromString(config.Tint);
                Background = config.Tint.ToBrush();
                this.Width = SystemParameters.WorkArea.Width;
                this.Height = SystemParameters.WorkArea.Height;
                this.Top = 0;
                this.Left = 0;
                background.Margin = new Thickness(SystemParameters.WorkArea.Width / 2 - 150, 0, 0, 0);
            }
            else
            {
                usingTint = false;
                this.Left = SystemParameters.WorkArea.Width / 2 - 150;
            }

            background.Background = config.Background.ToBrush();
            devName.Content = config.StaticName;
            devName.Foreground = config.NameForeground.ToBrush();
            media.Source = new Uri(config.MediaLocation);
            done.Content = config.ButtonText;
            done.Background = config.ButtonBackground.ToBrush();
            done.Foreground = config.ButtonForeground.ToBrush();

            Loaded += Card_Loaded;
        }

        private void Media_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (loop)
            {
                media.Position = TimeSpan.Zero;
                media.Play();
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            Dispatcher.Invoke(() => FadeOut());
        }

        private void Card_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() => FadeIn());
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Dispatcher.Invoke(() => FadeOut());
        }

        private void done_Click(object sender, RoutedEventArgs e)
        {
            Window_MouseDown(sender, null);
        }

        private async void FadeIn()
        {
            Opacity = 0;
            Helper.SetActiveWindow(currentWin);
            await Task.Delay(500);
            Helper.SetActiveWindow(currentWin);
            Opacity = 1;
            if (bottomSpawn)
            {
                if (usingTint)
                {
                    for (int i = (int)SystemParameters.WorkArea.Height + 300; i >= (int)SystemParameters.WorkArea.Height - 300; i -= 40)
                    {
                        background.Margin = new Thickness(background.Margin.Left, i, 0, 0);
                        Background = new SolidColorBrush(tintBack) { Opacity = 1 - (double)Math.Round(Decimal.Divide((i - 780), 600), 2) };
                        await Task.Delay(2);
                    }
                }
                else
                {
                    for (int i = (int)SystemParameters.WorkArea.Height + 300; i >= (int)SystemParameters.WorkArea.Height - 300; i -= 40)
                    {
                        this.Top = i;
                        await Task.Delay(2);
                    }
                }
            }
            else
            {
                if (usingTint)
                {
                    background.Margin = new Thickness(SystemParameters.WorkArea.Width / 2 - 150, SystemParameters.WorkArea.Height / 2 - 150, 0, 0);

                    for (float i = 0; i <= 1; i += 0.1f)
                    {
                        this.Opacity = i;
                        await Task.Delay(10);
                    }
                }
                else
                {
                    this.Top = SystemParameters.WorkArea.Height / 2 - 150;

                    for (float i = 0; i <= 1; i += 0.1f)
                    {
                        this.Opacity = i;
                        await Task.Delay(10);
                    }
                }
                this.Opacity = 1;
            }
            timer.Start();
        }

        private async void FadeOut()
        {
            if (bottomSpawn)
            {
                if (usingTint)
                {
                    for (int i = (int)SystemParameters.WorkArea.Height - 300; i <= (int)SystemParameters.WorkArea.Height + 300; i += 40)
                    {
                        background.Margin = new Thickness(background.Margin.Left, i, 0, 0);
                        Background = new SolidColorBrush(tintBack) { Opacity = 1 - (double)Math.Round(Decimal.Divide((i - 780), 600), 2) };
                        await Task.Delay(2);
                    }
                }
                else
                {
                    for (int i = (int)SystemParameters.WorkArea.Height - 300; i <= (int)SystemParameters.WorkArea.Height + 300; i += 40)
                    {
                        this.Top = i;
                        await Task.Delay(2);
                    }
                }
            }
            else
            {
                for (float i = 1; i >= 0; i -= 0.1f)
                {
                    this.Opacity = i;
                    await Task.Delay(10);
                }
            }
            Environment.Exit(0);
            this.Close();
        }
    }
}
