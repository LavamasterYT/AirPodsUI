using AirPodsUI.Configurator.Cards;
using AirPodsUI.Configurator.Configuration;
using Serilog;
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
            Log.Logger = new LoggerConfiguration().WriteTo.Console().WriteTo.File($"{Helper.LogsFolder}\\AirPodsUI-{DateTime.Now:MM-dd-yyy hh-mm-ss-ff}.txt").CreateLogger();
            Log.Information("I don't know proper ways to log info, im just gonna log information on every little thing lol");

            try
            {
                Log.Information("Checking main folder.");
                if (!Directory.Exists(Helper.AirPodsUIFolder))
                {
                    Directory.CreateDirectory(Helper.AirPodsUIFolder);
                }
                Log.Information("Checking templates folder.");
                if (!Directory.Exists(Helper.TemplateFolder))
                {
                    Directory.CreateDirectory(Helper.TemplateFolder);
                }
                Log.Information("Checking assets folder.");
                if (!Directory.Exists(Helper.AssetsFolder))
                {
                    Directory.CreateDirectory(Helper.AssetsFolder);
                }
                Log.Information("Checking my folder lol");
                if (!Directory.Exists(Helper.LogsFolder))
                {
                    Directory.CreateDirectory(Helper.LogsFolder);
                }
                Log.Information("Checking for paired devices.");
                if (!File.Exists(Helper.PairedDevicesFile))
                {
                    File.WriteAllText(Helper.PairedDevicesFile, PDSerialize.ToJson(new PairedDevicesJson() { Devices = new List<Device>() }));
                }

                Log.Information("Checking if there are any templates that need converting.");
                bool detected = false;
                foreach (var i in Directory.GetFiles(Helper.TemplateFolder))
                {
                    if (Path.GetExtension(i).ToLower() == ".json")
                    {
                        Log.Information("Looks like there is one!");
                        detected = true;
                        ConfigParser.JsonToCard(i);
                        File.Delete(i);
                    }
                }
                if (detected)
                    MessageBox.Show("AirPodsUI has detected a old template file. You may have to change the 'MediaLocation' property in the template configurator or by editing the file.", "Notice", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "We couldn't convert a template or something went wrong idk.");
                Helper.Error("Error", "Unable to check necessary directories/files. The program might not work correctly.");
            }

            this.Resources[AdonisUI.Colors.AccentColor] = SystemParameters.WindowGlassColor;
            Log.Information("Changed some colors.");

            if (e.Args.Length == 2)
            {
                Log.Information("Some args or something was detected. {0}", e.Args);
                CLArgs.Parse(e.Args);
            }
            else
            {
                Log.Information("Launching main window.");
                new MainWindow();
            }
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Fatal(e.Exception, "Looks like there was a unknown error. oops");
            Helper.Error("AirPodsUI", "An unknown error has occurred. Please contact the dev for help.");
            e.Handled = true;
        }
    }
}
