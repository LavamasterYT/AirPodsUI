using System;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AirPodsUI.Core
{
    public static class Logger
    {
        public static string LogFile { get; set; }
        public static string Name { get; set; }

        public static Exception CreateLog(string name)
        {
            try
            {
                Directory.CreateDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\AirPodsUI\\Logs");

                Name = name;
                LogFile = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\AirPodsUI\\Logs\\{name} {DateTime.Now.ToString("MM-dd-yy_HH-mm-ss")}.log";

                File.Create(LogFile).Close();

                return null;
            }
            catch (Exception e)
            {
                return e;
            }
        }

        public static void Log(string text, Exception e = null)
        {
            if (e == null)
            {
                Log(LogType.Information, text);
            }
            else
            {
                Log(LogType.Warning, text, e);
            }
        }

        public static void Log(LogType type, string text, Exception e = null)
        {
            try
            {
                FileStream log = File.Open(LogFile, FileMode.Open);

                log.Seek(0, SeekOrigin.End);

                log.Write(Encoding.ASCII.GetBytes($"[{Name}@{DateTime.Now.ToString("MM/dd/yy HH:mm:ss:ff")}|{(type == LogType.Error ? type.ToString().ToUpper() : type.ToString())}] {text}\n"));
                if (e != null)
                {
                    log.Write(Encoding.ASCII.GetBytes($"{Convert.ToString(e)}\n"));
                }

                log.Flush();
                log.Close();
                log.Dispose();
            }
            catch (Exception ee)
            {
                EventLog.WriteEntry("AirPodsUI", Convert.ToString(ee), EventLogEntryType.Error);
            }
        }
    }

    public enum LogType
    {
        Information,
        Warning,
        Error
    }
}
