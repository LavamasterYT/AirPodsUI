using System;
using System.Collections.Generic;
using System.Text;

namespace AirPodsUI.Core.Models
{
    public class PnPDevice
    {
        public string Name { get; set; }
        public string PNPDeviceID { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            PnPDevice dev = obj as PnPDevice;

            if (dev == null)
                return false;

            return Equals(dev);
        }

        public bool Equals(PnPDevice x)
        {
            if (x == null)
                return false;

            return x.PNPDeviceID == PNPDeviceID;
        }
    }
}
