using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ACS.AMS.DAL
{
    public static class LogHelper
    {
        public static string TraceLogLocation { get; set; }

        public static string CurrentTraceLogLocation { get; private set; }

        public static bool EnableTraceLog { get; set; } = true;

        private static DateTime lastLogInitTime = DateTime.MinValue;

        static LogHelper()
        {

        }

        public static void InitLog()
        {
            if (!EnableTraceLog) return;

            try
            {
                var now = DateTime.Now;
                var cLogDt = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
                if (lastLogInitTime != cLogDt)
                {
                    lastLogInitTime = cLogDt;

                    CurrentTraceLogLocation = Path.Combine(TraceLogLocation, cLogDt.ToString("yyyyMMdd"));
                    if (!Directory.Exists(CurrentTraceLogLocation))
                        Directory.CreateDirectory(CurrentTraceLogLocation);

                    var fileName = Path.Combine(CurrentTraceLogLocation, $"transactionLog_{cLogDt.ToString("HHmm")}.txt");

                    Trace.AutoFlush = false;

                    Trace.Listeners.Clear();
                    Trace.Listeners.Add(new TextWriterTraceListener(fileName));

                    AddTraceLog("New Log", "Log File Initiated");

                    Trace.Flush();
                    Trace.AutoFlush = true;
                }
            }
            catch (Exception ex) { }
        }

        private static void AddTraceLog(string message)
        {
            if (!EnableTraceLog) return;

            Trace.WriteLine(message);
        }

        public static void AddTraceLog(string functionality, string message, [CallerMemberName] string method = null,
            [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = -1, string logType = "TRACE")
        {
            if (!EnableTraceLog) return;

            if (!string.IsNullOrEmpty(filePath))
                filePath = Path.GetFileName(filePath);

            string logMsg = $"{logType} {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff")} => {message}. (M:{method}, F:{functionality}, S:{filePath}, L:{lineNumber})";
            LogHelper.AddTraceLog(logMsg);
        }
    }
}
