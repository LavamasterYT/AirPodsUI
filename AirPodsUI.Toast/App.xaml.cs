using System;
using System.Data;
using System.Windows;

namespace AirPodsUI.Toast
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            ToastSettings settings = new ToastSettings();

            try
            {
                for (int i = 0; i < e.Args.Length; i++)
                {
                    switch (e.Args[i])
                    {
                        case "--offset":
                            i += 1;
                            settings.Offset = int.Parse(e.Args[i]);
                            break;
                        case "--name":
                            i += 1;
                            settings.Name = e.Args[i];
                            break;
                        case "--dark-mode":
                            settings.DarkMode = true;
                            break;
                        default:
                            throw new SyntaxErrorException();
                    }
                }
            }
            catch (Exception)
            {
                Shutdown();
                return;
            }

            new PencilToast(settings).Show();
        }
    }
}
