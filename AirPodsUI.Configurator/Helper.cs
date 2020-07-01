using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;

namespace AirPodsUI.Configurator
{
    public static class Helper
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        public static IntPtr ActiveWindowHandle
        {
            get
            {
                return GetForegroundWindow();
            }
        }

        public static void SetActiveWindow(IntPtr hWnd)
        {
            SetForegroundWindow(hWnd);
        }

        public static void Error(string title, string message) => MessageBox.Show(message, title, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);

        /// <summary>
        /// Get the AirPodsUI folder
        /// </summary>
        public static string AirPodsUIFolder { get; set; } = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\AirPodsUI\\";
        
        /// <summary>
        /// The executing app directory
        /// </summary>
        public static string AppDirectory { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

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

        private static string numberPattern = " ({0})";

        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
   => self.Select((item, index) => (item, index));

        public static string NextAvailableFilename(string path)
        {
            // Short-cut if already available
            if (!File.Exists(path))
                return path;

            // If path has extension then insert the number pattern just before the extension and return next filename
            if (Path.HasExtension(path))
                return GetNextFilename(path.Insert(path.LastIndexOf(Path.GetExtension(path)), numberPattern));

            // Otherwise just append the pattern to the path and return next filename
            return GetNextFilename(path + numberPattern);
        }

        private static string GetNextFilename(string pattern)
        {
            string tmp = string.Format(pattern, 1);
            if (tmp == pattern)
                throw new ArgumentException("The pattern must include an index place-holder", "pattern");

            if (!File.Exists(tmp))
                return tmp; // short-circuit if no matches

            int min = 1, max = 2; // min is inclusive, max is exclusive/untested

            while (File.Exists(string.Format(pattern, max)))
            {
                min = max;
                max *= 2;
            }

            while (max != min + 1)
            {
                int pivot = (max + min) / 2;
                if (File.Exists(string.Format(pattern, pivot)))
                    min = pivot;
                else
                    max = pivot;
            }

            return string.Format(pattern, max);
        }
    }
}
