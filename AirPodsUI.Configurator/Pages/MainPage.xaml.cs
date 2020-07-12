using Serilog;
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
                Log.Information("Loding README");
                contents.Text = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\README.txt");
                Log.Information("Loaded README.");
            }
            catch (Exception e)
            {
                Log.Error(e, "Unable to load the README file.");
                contents.Text = "Unable to locate the README.txt file.";
            }
        }
    }
}
