using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Plugin.Services;
using System;
using Lumina.Excel.Sheets;
using ImPlotNET;
using System.Reflection.Metadata.Ecma335;

namespace SimpleLogs.Chat
{
    public class LogEntry{
        public string message;
        public string timestamp;
        public LogEntry(string message, string timestamp)
        {
            this.message = message;
            this.timestamp = timestamp;
        }
    }
    public class DebugLog
    {
        private Plugin plugin;
        private List<LogEntry> log;
        public DebugLog(Plugin plugin)
        {
            this.plugin = plugin;
            this.log = new List<LogEntry>();
            this.AddEntry("Debug logging started.");
        }

        public void AddEntry(string message)
        {
            log.Add(new LogEntry(message, plugin.timer.GetElapsedTime().TotalSeconds.ToString()));
            //plugin.ChatGui.Print($"[Debug] {message}");
        }

        public void ExportLog()
        {
            string appDataPath;
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                appDataPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.config/SimpleLogs";
            }
            else
            {
                appDataPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/SimpleLogs";

            }
            if (!System.IO.Directory.Exists(appDataPath))
            {
                System.IO.Directory.CreateDirectory(appDataPath);
            }
            string filePath = $"{appDataPath}/DebugLog.txt";
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath))
            {
                foreach (var entry in log)
                {
                    file.WriteLine($"{entry.timestamp}: {entry.message}");
                }
            }
            plugin.ChatGui.Print($"[Debug] Log exported to {filePath}");
        }
    }


}