using System;
using System.Linq;
using System.Threading.Tasks;
using Boxsie.Wrapplication.Config;
using Boxsie.Wrapplication.Config.Contracts;
using Boxsie.Wrapplication.Logging;
using Boxsie.Wrapplication.Net;
using Boxsie.Wrapplication.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Boxsie.Wrapplication
{
    public static class Bx
    {
        public static IBxApp App { get; private set; }

        public static void Start()
        {
            Task.Run(async () => await App.StartAsync());
        }

        public static void ConfigureServices<T>(IServiceCollection services)
        {
            services.AddSingleton(typeof(IBxApp), typeof(T));
            
            services.AddLogging();
            services.AddSingleton<IBxLogger, BxLogger>();
            services.AddSingleton<ILogger, BxLogger>(x => (BxLogger)x.GetService<IBxLogger>());

            services.AddSingleton<RepositoryService>();
            services.AddSingleton<HttpClientFactory>();

            AddConfigs(services);
        }
        
        public static void Configure(IServiceProvider serviceProvider)
        {
            Cfg.InitialiseConfig(serviceProvider);

            var logFactory = serviceProvider.GetService<ILoggerFactory>();
            logFactory.AddProvider(new BxLoggerProvider(Cfg.GetConfig<GeneralConfig>()));

            serviceProvider.GetService<RepositoryService>().EnsureDbCreated();

            App = serviceProvider.GetService<IBxApp>();
        }

        private static void AddConfigs(IServiceCollection services)
        {
            services.AddSingleton<GeneralConfig>(x => Cfg.ConfigFactory<GeneralConfig>());
            services.AddSingleton<IConfig, GeneralConfig>(x => x.GetService<GeneralConfig>());

            var contract = typeof(IConfig);

            var configTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes()
                    .Where(y => contract.IsAssignableFrom(y) && !y.IsInterface && !y.IsAbstract && y != typeof(GeneralConfig)));

            foreach (var configType in configTypes)
            {
                services.AddSingleton(configType, x => Cfg.ConfigFactory(configType));
                services.AddSingleton(contract, x => x.GetService(configType));
            }
        }
    }
}
