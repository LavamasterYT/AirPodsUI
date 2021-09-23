using System;
using System.Management;
using AirPodsUI.Core.Models;
using System.Collections.Generic;

namespace AirPodsUI.Core
{
    public class PNP
    {
        public static List<PnPDevice> GetPNPDevices()
        {
            List<PnPDevice> result = new List<PnPDevice>();

            ManagementObjectCollection col;

            using (var searcher = new ManagementObjectSearcher(@"SELECT * FROM Win32_PnPEntity"))
            {
                col = searcher.Get();
            }

            foreach (var i in col)
            {
                try
                {
                    PnPDevice dev = new PnPDevice()
                    {
                        Name = (string)i.GetPropertyValue("Description"),
                        PNPDeviceID = (string)i.GetPropertyValue("PNPDeviceID")
                    };

                    result.Add(dev);
                }
                catch (Exception) { }
            }

            col.Dispose();

            return result;
        }
    }
}
