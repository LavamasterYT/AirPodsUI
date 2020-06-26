using AirPodsUI.Service.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace AirPodsUI.Service
{
    class USBComparer : IEqualityComparer<USBDevice>
    {
        public bool Equals(USBDevice x, USBDevice y)
        {
            return x.DeviceID == y.DeviceID;
        }

        public int GetHashCode(USBDevice obj)
        {
            return obj.DeviceID.GetHashCode();
        }
    }
}
