using System;
using System.Data.SQLite;
using System.IO;
using Boxsie.Wrapplication.Config;
using Boxsie.Wrapplication.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Boxsie.Wrapplication.Repository
{
    public class RepositoryService
    {
        private readonly ILogger<RepositoryService> _logger;
        private readonly GeneralConfig _generalConfig;

        public RepositoryService(ILogger<RepositoryService> logger, GeneralConfig generalConfig)
        {
            _logger = logger;
            _generalConfig = generalConfig;
        }

        public void EnsureDbCreated()
        {
            _logger.LogInformation($"Looking for database...");

            var dbPath = Path.Combine(_generalConfig.UserConfig.UserDataPath, _generalConfig.DbFilename);

            _logger.LogInformation($"Looking for database at {dbPath}");

            if (File.Exists(dbPath))
                return;

            _logger.LogInformation($"Database not found, creating...");

            SQLiteConnection.CreateFile(dbPath);

            _logger.LogInformation($"Database created");
        }
    }

    public static class RepositoryServiceHelpers
    {
        public static void RegisterEntity<T>(this IServiceProvider serviceProvider)
        {
            var repo = serviceProvider.GetService<IRepository<T>>();

            repo.CreateTable();
        }
    }
}