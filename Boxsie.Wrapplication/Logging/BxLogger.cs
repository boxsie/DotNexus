using System;
using System.IO;
using System.Threading.Tasks;
using Boxsie.Wrapplication.Config;
using Microsoft.Extensions.Logging;

namespace Boxsie.Wrapplication.Logging
{
    public class BxLogger : IBxLogger, ILogger
    {
        private static readonly object Lock = new object();

        private readonly GeneralConfig _config;

        public BxLogger(GeneralConfig config)
        {
            _config = config;

            if (!Directory.Exists(_config.UserConfig.LogOutputPath))
                Directory.CreateDirectory(_config.UserConfig.LogOutputPath);
        }

        public void WriteLine(string message, LogLevel lvl = LogLevel.Debug)
        {
            lock (Lock)
            {
                var logMsg = CreateMessage(message, lvl);

                OutputToConsole(logMsg);

                SaveToDisk(GetLogfilename(), logMsg);
            }
        }

        private string GetLogfilename()
        {
            return Path.Combine(_config.UserConfig.LogOutputPath, $"{DateTime.Now:yy-MM-dd}.log");
        }

        private static string CreateMessage(string message, LogLevel lvl = LogLevel.Debug)
        {
            return $"[{DateTime.Now:T}][{lvl.ToString()[0]}]:{message}";
        }

        private static void OutputToConsole(string logMsg)
        {
            Console.WriteLine(logMsg);
        }

        private static void SaveToDisk(string fileName, string logMsg)
        {
            using (var sw = File.AppendText(fileName))
                sw.WriteLine(logMsg);
        }

        private static async Task SaveToDiskAsync(string fileName, string logMsg)
        {
            using (var sw = File.AppendText(fileName))
                await sw.WriteLineAsync(logMsg);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            WriteLine(formatter(state, exception), logLevel);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}
