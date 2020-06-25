using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AirPodsUI.Configurator.Configuration
{
    public class ConfigParser
    {
        public static PencilConfig ParseP(string file)
        {
            PencilConfig result = new PencilConfig();

            List<string> lines = new List<string>();
            using (StreamReader sr = new StreamReader(file))
            {
                while (!sr.EndOfStream)
                {
                    lines.Add(sr.ReadLine());
                }
            }

            foreach (var i in lines)
            {
                if (i.StartsWith('#') || string.IsNullOrEmpty(i))
                    continue;

                string[] line = i.Split('=');

                if (line.Length != 2)
                    throw new InvalidDataException();

                line[1] = line[1].Replace("$airpods", "pack://application:,,,/Assets/AirPods.png");
                line[1] = line[1].Replace("$tritone", "pack://application:,,,/Assets/tritone.mp3");
                line[1] = line[1].Replace("$usb", "pack://application:,,,/Assets/usb.png");
                line[1] = line[1].Replace("$bluetooth", "pack://application:,,,/Assets/bluetooth.png");

                switch (line[0])
                {
                    case "TemplateName":
                        result.Name = line[1];
                        break;
                    case "Background":
                        result.Background = line[1];
                        break;
                    case "DeviceNameTextForeground":
                        result.DeviceNameForeground = line[1];
                        break;
                    case "StatusTextForeground":
                        result.StatusForeground = line[1];
                        break;
                    case "StaticName":
                        result.StaticName = line[1];
                        break;
                    case "StatusText":
                        result.StatusText = line[1];
                        break;
                    case "IconLocation":
                        result.IconLocation = line[1];
                        break;
                    default:
                        throw new InvalidDataException();
                }    
            }

            return result;
        }

        public static NotificationConfig ParseN(string file)
        {
            NotificationConfig result = new NotificationConfig();

            List<string> lines = new List<string>();
            using (StreamReader sr = new StreamReader(file))
            {
                while (!sr.EndOfStream)
                {
                    lines.Add(sr.ReadLine());
                }
            }

            foreach (var i in lines)
            {
                if (i.StartsWith('#') || string.IsNullOrEmpty(i))
                    continue;

                string[] line = i.Split('=');

                if (line.Length != 2)
                    throw new InvalidDataException();

                line[1] = line[1].Replace("$airpods", "pack://application:,,,/Assets/AirPods.png");
                line[1] = line[1].Replace("$tritone", "pack://application:,,,/Assets/tritone.mp3");
                line[1] = line[1].Replace("$usb", "pack://application:,,,/Assets/usb.png");
                line[1] = line[1].Replace("$bluetooth", "pack://application:,,,/Assets/bluetooth.png");

                switch (line[0])
                {
                    case "TemplateName":
                        result.Name = line[1];
                        break;
                    case "Background":
                        result.Background = line[1];
                        break;
                    case "CaptionForeground":
                        result.CaptionForeground = line[1];
                        break;
                    case "AppNameColor":
                        result.AppNameForeground = line[1];
                        break;
                    case "StatusTextForeground":
                        result.StatusForeground = line[1];
                        break;
                    case "StaticName":
                        result.StaticName = line[1];
                        break;
                    case "StatusText":
                        result.StatusText = line[1];
                        break;
                    case "AppName":
                        result.AppName = line[1];
                        break;
                    case "IconLocation":
                        result.IconPath = line[1];
                        break;
                    case "NotificationSound":
                        result.NotifSound = line[1];
                        break;
                    default:
                        throw new InvalidDataException();
                }
            }

            return result;
        }
    }
}
