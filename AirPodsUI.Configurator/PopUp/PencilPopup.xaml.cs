using System;
using System.Collections.Generic;
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

namespace AirPodsUI.Configurator.PopUp
{
    /// <summary>
    /// Interaction logic for PencilPopup.xaml
    /// </summary>
    public partial class PencilPopup : Window
    {
        Timer timer;
        bool FadingBottom;

        public PencilPopup()
        {
            InitializeComponent();
            timer = new Timer();

            timer.Interval = 5000;
            timer.Elapsed += Timer_Elapsed;

            this.FadingBottom = false;

            this.Left = (SystemParameters.WorkArea.Width / 2) - (this.Width / 2);
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
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
            if (!FadingBottom)
                Dispatcher.Invoke(() => Fade(-60, 20, 10, 10, false, false));
            timer.Start();
        }
    }
}
