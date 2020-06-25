using System;
using System.Collections.Generic;
using System.Text;

namespace AirPodsUI.Configurator.Configuration
{
    public class CardConfig
    {
        public string Name { get; set; } = "Card Default";

        public string Background { get; set; } = "#FFFFFF";

        public string NameForeground { get; set; } = "#000000";

        public string ButtonForeground { get; set; } = "#000000";

        public string ButtonBackground { get; set; } = "#AAAAAA";

        public string Tint { get; set; } = "";

        public string ButtonText { get; set; } = "Done";

        public string Location { get; set; } = "Center";

        public string StaticName { get; set; } = "";

        public string MediaPath { get; set; } = "pack://application:,,,/Assets/pro.mp4";

        public string StretchMode { get; set; } = "None";

        public string TimeOut { get; set; } = "5000";

        public string Loop { get; set; } = "true";
    }
}
/*
Name=Pencil Dark Mode

Background=#101010

NameForeground=#EEEEEE

ButtonForeground=#999999

ButtonBackground=#FFFFFF

Tint=#000000

ButtonText=Done

StaticName=Headphones

IconLocation=$docs\Assets\headphones.png
*/