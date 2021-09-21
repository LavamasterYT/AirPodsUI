using System;

namespace AirPodsUI.Toast
{
    public class ToastSettings
    {
        public int Offset { get; set; } = 10;
        public string Name { get; set; } = "USB Device Device";
        public bool DarkMode { get; set; } = true;
        public DeviceType Device { get; set; } = DeviceType.USB;
    }
}
