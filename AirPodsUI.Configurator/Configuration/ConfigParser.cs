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

                line[1] = ReplaceVariables(line[1]);

                switch (line[0])
                {
                    case "TemplateName":
                        result.TemplateName = line[1];
                        break;
                    case "Background":
                        result.Background = line[1];
                        break;
                    case "DeviceNameTextForeground":
                        result.DeviceNameTextForeground = line[1];
                        break;
                    case "StatusTextForeground":
                        result.StatusTextForeground = line[1];
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

                line[1] = ReplaceVariables(line[1]);

                switch (line[0])
                {
                    case "TemplateName":
                        result.TemplateName = line[1];
                        break;
                    case "Background":
                        result.Background = line[1];
                        break;
                    case "CaptionForeground":
                        result.CaptionForeground = line[1];
                        break;
                    case "AppNameColor":
                        result.AppNameColor = line[1];
                        break;
                    case "StatusTextForeground":
                        result.StatusTextForeground = line[1];
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
                        result.IconLocation = line[1];
                        break;
                    case "NotificationSound":
                        result.NotificationSound = line[1];
                        break;
                    default:
                        throw new InvalidDataException();
                }
            }

            return result;
        }

        public static CardConfig ParseC(string file)
        {
            CardConfig result = new CardConfig();

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

                line[1] = ReplaceVariables(line[1]);

                switch (line[0])
                {
                    case "TemplateName":
                        result.TemplateName = line[1];
                        break;
                    case "Background":
                        result.Background = line[1];
                        break;
                    case "NameForeground":
                        result.NameForeground = line[1];
                        break;
                    case "ButtonForeground":
                        result.ButtonForeground = line[1];
                        break;
                    case "ButtonBackground":
                        result.ButtonBackground = line[1];
                        break;
                    case "StaticName":
                        result.StaticName = line[1];
                        break;
                    case "Tint":
                        result.Tint = line[1];
                        break;
                    case "ButtonText":
                        result.ButtonText = line[1];
                        break;
                    case "Location":
                        result.Location= line[1];
                        break;
                    case "StretchMode":
                        result.StretchMode = line[1];
                        break;
                    case "MediaLocation":
                        result.MediaLocation = line[1];
                        break;
                    case "TimeOut":
                        result.TimeOut = line[1];
                        break;
                    case "Loop":
                        result.Loop = line[1];
                        break;
                    default:
                        throw new InvalidDataException();
                }
            }

            return result;
        }

        private static string ReplaceVariables(string input)
        {
            input = input.Replace("$airpods", "pack://application:,,,/Assets/AirPods.png");
            input = input.Replace("$tritone", "pack://application:,,,/Assets/tritone.mp3");
            input = input.Replace("$usb", "pack://application:,,,/Assets/usb.png");
            input = input.Replace("$bluetooth", "pack://application:,,,/Assets/bluetooth.png");
            input = input.Replace("$pro", "pack://application:,,,/Assets/pro.mp4");
            return input;
        }
    }
}
