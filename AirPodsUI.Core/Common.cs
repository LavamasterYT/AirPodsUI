using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AirPodsUI.Core
{
    class Common
    {
        public static string MainDirectory
        {
            get
            {
                string dir = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\AirPodsUI";

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                return dir;
            }
        }

        public static string DevicesFile
        {
            get
            {
                string file = $"{MainDirectory}\\devices.json";

                if (!File.Exists(file))
                {
                    FileStream f = File.Create(file);
                    f.Write(Encoding.ASCII.GetBytes("{\n}\n"), 0, 4);
                    f.Close();
                }

                return file;
            }
        }
    }
}
