using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace Alzayed.Utilities {
    public class Logger {
        private string path = "";
        private string file = "";

        public Logger(string filePath, string fileName) {
            path = filePath;
            file = fileName;
            BuildPath();
        }

        private void BuildPath() {
            try {
                string appName = Assembly.GetEntryAssembly().GetName().Name;
                string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month);
                string day = DateTime.Now.Day.ToString();
                string dir = Path.Combine(path,appName,DateTime.Now.Year.ToString(),monthName,day);
                Directory.CreateDirectory($"{dir}");
                path = $"{dir}\\{file}";
            } catch(Exception) {
            }
        }

        public bool LogError(Exception ex) {
            try {
                LogLine(ex.Message);
                LogLine(ex.StackTrace);
                return true;
            } catch(Exception) {
                return false;
            }
        }

        public bool LogLine(string lineToLog) {
            try {
                using(var sw = GetStreamWriter(append: true)) {
                    sw.WriteLine($"[{GetCurrentTime()}] {lineToLog}");
                    sw.Close();
                }
                return true;
            } catch(Exception) {
                return false;
            }
        }

        public bool LogListOfLines(IEnumerable<string> Lines) {
            try {
                using(var sw = GetStreamWriter(append: true)) {
                    foreach(var line in Lines)
                        sw.WriteLine($"[{GetCurrentTime()}] {line}");
                    sw.Close();
                }
                return true;
            } catch(Exception) {
                return false;
            }
        }

        private string GetCurrentTime()
            => DateTime.Now.ToLongTimeString();


        public bool CleanLogFile() {
            try {
                using(var sw = GetStreamWriter())
                    sw.Close();
                return true;
            } catch(Exception) {
                return false;
            }
        }

        private StreamWriter GetStreamWriter(bool append = false) {
            var sw = new StreamWriter(path,append);
            return sw;
        }

    }
}
