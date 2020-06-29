using AirPodsUI.Configurator.Cards;
using System.Collections.Generic;

namespace AirPodsUI.Configurator.Configuration
{
    public class CreateConfigFile
    {
        public static string Create(PencilConfig config)
        {
            List<string> result = new List<string>();

            result.Add($"{nameof(config.TemplateName)}={config.TemplateName}");
            result.Add($"{nameof(config.Background)}={config.Background}");
            result.Add($"{nameof(config.DeviceNameTextForeground)}={config.DeviceNameTextForeground}");
            result.Add($"{nameof(config.StatusTextForeground)}={config.StatusTextForeground}");
            result.Add($"{nameof(config.DeviceNameTextForeground)}={config.DeviceNameTextForeground}");
            result.Add($"{nameof(config.StaticName)}={config.StaticName}");
            result.Add($"{nameof(config.StatusText)}={config.StatusText}");
            result.Add($"{nameof(config.IconLocation)}={config.IconLocation}");

            return string.Join("\n", result);
        }

        public static string Create(CardConfig config)
        {
            List<string> result = new List<string>();

            result.Add($"{nameof(config.TemplateName)}={config.TemplateName}");
            result.Add($"{nameof(config.Background)}={config.Background}");
            result.Add($"{nameof(config.NameForeground)}={config.NameForeground}");
            result.Add($"{nameof(config.ButtonForeground)}={config.ButtonForeground}");
            result.Add($"{nameof(config.ButtonBackground)}={config.ButtonBackground}");
            if (config.Tint != "") result.Add($"{nameof(config.Tint)}={config.Tint}");
            result.Add($"{nameof(config.ButtonText)}={config.ButtonText}");
            result.Add($"{nameof(config.StaticName)}={config.StaticName}");
            result.Add($"{nameof(config.Location)}={config.Location}");
            result.Add($"{nameof(config.StretchMode)}={config.StretchMode}");
            result.Add($"{nameof(config.Loop)}={config.Loop}");
            result.Add($"{nameof(config.MediaLocation)}={config.MediaLocation}");
            result.Add($"{nameof(config.TimeOut)}={config.TimeOut}");

            return string.Join("\n", result);
        }

        public static string Create(NotificationConfig config)
        {
            List<string> result = new List<string>();

            result.Add($"{nameof(config.TemplateName)}={config.TemplateName}");
            result.Add($"{nameof(config.Background)}={config.Background}");
            result.Add($"{nameof(config.CaptionForeground)}={config.CaptionForeground}");
            result.Add($"{nameof(config.AppNameColor)}={config.AppNameColor}");
            result.Add($"{nameof(config.StatusTextForeground)}={config.StatusTextForeground}");
            result.Add($"{nameof(config.StaticName)}={config.StaticName}");
            result.Add($"{nameof(config.StatusText)}={config.StatusText}");
            result.Add($"{nameof(config.AppName)}={config.AppName}");
            result.Add($"{nameof(config.IconLocation)}={config.IconLocation}");
            result.Add($"{nameof(config.NotificationSound)}={config.NotificationSound}");

            return string.Join("\n", result);
        }
    }
}
