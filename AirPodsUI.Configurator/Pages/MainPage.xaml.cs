using System;
using System.IO;
using System.Reflection;
using System.Windows.Controls;

namespace AirPodsUI.Configurator.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            try
            {
                contents.Text = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\README.txt");
            }
            catch (Exception)
            {

            }
        }
    }
}
