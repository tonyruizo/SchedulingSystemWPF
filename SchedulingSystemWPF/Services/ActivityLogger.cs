using System;
using System.Globalization;
using System.IO;

namespace SchedulingSystemWPF.Services
{
    public class ActivityLogger
    {
        private static readonly string LogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            "..",
            "..",
            "Login_History.txt");

        public static void LogActivity(string username, string actionKey)
        {
            try
            {
                CultureInfo culture = CultureInfo.CurrentUICulture;

                string timestamp = DateTime.Now.ToString(culture.DateTimeFormat.ShortDatePattern + " " + culture.DateTimeFormat.ShortTimePattern);
                string message = string.Format(actionKey, username);
                string logEntry = $"{timestamp}: {message}";

                string directory = Path.GetDirectoryName(LogFilePath);

                // If exists
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.AppendAllText(LogFilePath, logEntry + Environment.NewLine);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Failed to write to activity log: {ex.Message}");
            }
        }
    }
}
