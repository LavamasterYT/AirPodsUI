using AirPodsUI.Configurator.Cards;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AirPodsUI.Configurator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// This method is going to be used for checking if the proper directory and files exist.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (!Directory.Exists(Helper.AirPodsUIFolder))
            {
                Directory.CreateDirectory(Helper.AirPodsUIFolder);
            }

            if (!Directory.Exists(Helper.TemplateFolder))
            {
                Directory.CreateDirectory(Helper.TemplateFolder);
            }

            if (!Directory.Exists(Helper.AssetsFolder))
            {
                Directory.CreateDirectory(Helper.AssetsFolder);
            }

            if (!Directory.Exists(Helper.LogsFolder))
            {
                Directory.CreateDirectory(Helper.LogsFolder);
            }

            if (!File.Exists(Helper.PairedDevicesFile))
            {
                File.WriteAllText(Helper.PairedDevicesFile, PDSerialize.ToJson(new PairedDevicesJson() { Devices = new List<Device>() }));
            }

            if (e.Args.Length == 2)
            {
                CLArgs.Parse(e.Args);
            }
            else
            {
                new MainWindow().Show();
            }
        }
    }
}
