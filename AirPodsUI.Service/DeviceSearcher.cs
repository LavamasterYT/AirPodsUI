using AirPodsUI.Service.Models;
using System;
using System.Collections.Generic;
using System.Management;
using System.Text;

namespace AirPodsUI.Service
{
    class DeviceSearcher
    {
        public static List<USBDevice> Search()
        {
            ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_USBHub");
            List<USBDevice> USBDevices = new List<USBDevice>();
            foreach (ManagementObject usb in mos.Get())
            {
                USBDevices.Add(new USBDevice { DeviceID = usb.Properties["DeviceID"].Value.ToString(), DeviceName = usb.Properties["Description"].Value.ToString() });
            }

            return USBDevices;
        }
    }
}
