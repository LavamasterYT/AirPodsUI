using System;
using System.Collections.Generic;
using System.Text;

namespace AirPodsUI.Configurator.Configuration
{
    public class PencilConfig
    {
        public string TemplateName { get; set; } = "Pencil Default";

        public string Background { get; set; } = "#252525";

        public string DeviceNameTextForeground { get; set; } = "#FFFFFF";

        public string StatusTextForeground { get; set; } = "#A9A9A9";

        public string StaticName { get; set; } = "";

        public string StatusText { get; set; } = "Connected";

        public string IconLocation { get; set; } = "pack://application:,,,/Assets/AirPods.png";
    }
}
/*
Name=Pencil Dark Mode

Background=#101010

NameForeground=#EEEEEE

StatusForeground=#999999

StaticName=Headphones

IconLocation=$airpods
*/