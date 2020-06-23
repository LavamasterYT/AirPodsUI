using System;

namespace AirPodsUI.Configurator
{
    public static class Helper
    {
        /// <summary>
        /// Get the AirPodsUI folder
        /// </summary>
        public static string AirPodsUIFolder { get; set; } = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\AirPodsUI\\";

        /// <summary>
        /// Get the PairedDevices.json file
        /// </summary>
        public static string PairedDevicesFile { get; set; } = $"{AirPodsUIFolder}PairedDevices.json";

        /// <summary>
        /// Get the AirPodsUI\Templates folder
        /// </summary>
        public static string TemplateFolder { get; set; } = $"{AirPodsUIFolder}Templates";

        /// <summary>
        /// Get the AirPodsUI\Assets folder
        /// </summary>
        public static string AssetsFolder { get; set; } = $"{AirPodsUIFolder}Assets";

        /// <summary>
        /// Get the AirPodsUI\Logs folder
        /// </summary>
        public static string LogsFolder { get; set; } = $"{AirPodsUIFolder}Logs";
    }
}
