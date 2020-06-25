using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace AirPodsUI.Configurator
{
    internal static class BrushExtension
    {
        public static Brush ToBrush(this string input)
        {
            var c = new BrushConverter();
            return (Brush)c.ConvertFromString(input);
        }
    }
}
