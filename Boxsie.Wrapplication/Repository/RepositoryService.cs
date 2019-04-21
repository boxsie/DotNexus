using System.Data.SQLite;
using System.IO;
using Boxsie.Wrapplication.Config;
using Boxsie.Wrapplication.Logging;
using Microsoft.Extensions.Logging;

namespace Boxsie.Wrapplication.Repository
{
    public class RepositoryService
    {
        private readonly IBxLogger _logger;
        private readonly GeneralConfig _generalConfig;

        public RepositoryService(IBxLogger logger, GeneralConfig generalConfig)
        {
            _logger = logger;
            _generalConfig = generalConfig;
        }

        public void EnsureDbCreated()
        {
            _logger.WriteLine($"Looking for database...", LogLevel.Information);

            var dbPath = Path.Combine(_generalConfig.UserConfig.UserDataPath, _generalConfig.DbFilename);

            _logger.WriteLine($"Looking for database at {dbPath}", LogLevel.Information);

            if (File.Exists(dbPath))
                return;

            _logger.WriteLine($"Database not found, creating...", LogLevel.Information);

            SQLiteConnection.CreateFile(dbPath);

            _logger.WriteLine($"Database created", LogLevel.Information);
        }
    }
}