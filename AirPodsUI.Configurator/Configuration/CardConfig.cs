using System;
using System.Collections.Generic;
using System.Text;

namespace AirPodsUI.Configurator.Configuration
{
    class CardConfig
    {
        public string Name { get; set; } = "Card Default";

        public string Background { get; set; } = "#FFFFFF";

        public string NameForeground { get; set; }

        public string ButtonForeground { get; set; }

        public string ButtonBackground { get; set; }

        public string Tint { get; set; }

        public string ButtonText { get; set; }

        public string StaticName { get; set; }

        public string MediaPath { get; set; }
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