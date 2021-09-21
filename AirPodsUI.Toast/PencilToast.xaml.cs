using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AirPodsUI.Toast
{
    public partial class PencilToast : Window
    {
        BrushConverter converter;
        ToastSettings settings;

        public PencilToast(ToastSettings cmdLine)
        {
            InitializeComponent();
            converter = new BrushConverter();
            settings = cmdLine;
        }

        private async void OnToastLoaded(object sender, RoutedEventArgs e)
        {
            Left = SystemParameters.PrimaryScreenWidth / 2 - Width / 2;
            Top = 0 - Height;

            tDeviceName.Text = settings.Name;

            if (settings.DarkMode)
            {
                tToast.Background = (Brush)converter.ConvertFromString("#252525");
                tDeviceName.Foreground = (Brush)converter.ConvertFromString("#DDDDDD");
                if (settings.Device == DeviceType.USB)
                    tIcon.Source = new BitmapImage(new Uri($"{AppDomain.CurrentDomain.BaseDirectory}\\Images\\usb_dark.png"));
                else
                    tIcon.Source = new BitmapImage(new Uri($"{AppDomain.CurrentDomain.BaseDirectory}\\Images\\bt_dark.png"));
            }
            else
            {
                if (settings.Device == DeviceType.USB)
                    tIcon.Source = new BitmapImage(new Uri($"{AppDomain.CurrentDomain.BaseDirectory}\\Images\\usb_light.png"));
                else
                    tIcon.Source = new BitmapImage(new Uri($"{AppDomain.CurrentDomain.BaseDirectory}\\Images\\bt_light.png"));
            }

            for (float i = 0; i < 1; i += 0.05f)
            {
                Top = Ease(i) * settings.Offset - Height;
                await Task.Delay(10);
            }

            await Task.Delay(2000);

            for (float i = 1; i > 0; i -= 0.05f)
            {
                Top = Ease(i) * settings.Offset - Height;
                await Task.Delay(10);
            }

            this.Close();
        }

        public float Ease(float x)
        {
            return (float)(-(Math.Cos(Math.PI * x) - 1) / 2);
        }
    }
}
