using System;
using System.Collections.Generic;
using System.Text;

namespace AirPodsUI.Configurator.Configuration
{
    public class NotificationConfig
    {
        public string Name { get; set; } = "Notification Default";

        public string Background { get; set; } = "#EF202020";

        public string CaptionForeground { get; set; } = "#FFFFFF";

        public string AppNameForeground { get; set; } = "#808080";

        public string StatusForeground { get; set; } = "#FFFFFF";

        public string AppName { get; set; } = "AIRPODSUI";

        public string StaticName { get; set; } = "";

        public string StatusText { get; set; } = "Connected";

        public string IconPath { get; set; } = "pack://application:,,,/Assets/AirPods.png";

        public string NotifSound { get; set; } = "pack://application:,,,/Assets/tritone.mp3";
    }
}
/*
Name=Pencil Dark Mode

Background=#101010

TextForeground=#EEEEEE

AppNameColor=#FFFFFF

StatusForeground=#999999

StaticName=Headphones

IconLocation=$docs\Assets\headphones.png

NotificationSound=$docs\Assets\tri-tone.mp3
*/