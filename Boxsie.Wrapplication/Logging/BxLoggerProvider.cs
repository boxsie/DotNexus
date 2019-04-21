using System.Collections.Concurrent;
using Boxsie.Wrapplication.Config;
using Microsoft.Extensions.Logging;

namespace Boxsie.Wrapplication.Logging
{
    public class BxLoggerProvider : ILoggerProvider
    {
        private readonly GeneralConfig _config;
        private readonly ConcurrentDictionary<string, BxLogger> _loggers;

        public BxLoggerProvider(GeneralConfig config)
        {
            _config = config;
            _loggers = new ConcurrentDictionary<string, BxLogger>();
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new BxLogger(_config));
        }

        public void Dispose()
        {
            _loggers.Clear();
        }
    }
}