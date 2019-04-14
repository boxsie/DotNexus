using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using DotNexus.Accounts;
using DotNexus.Accounts.Models;
using DotNexus.Assets;
using DotNexus.Assets.Models;
using DotNexus.Core;
using DotNexus.Ledger;
using DotNexus.Nexus;
using DotNexus.Tokens;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using Xunit.Abstractions;
using ILogger = NLog.ILogger;
using LogLevel = NLog.LogLevel;

namespace DotNexus.Tests
{
    public class NexusServiceFixture : IDisposable
    {
        public LedgerService LedgerService { get; private set; }
        public AccountService AccountService { get; private set; }
        public AssetService AssetService { get; private set; }
        public TokenService TokenService { get; private set; }
        public BlockNotifyJob BlockNotify { get; private set; }

        public const bool IsApiSession = true;

        public void Configure(ITestOutputHelper outputHelper)
        {
            var config = new LoggingConfiguration();
            config.AddRule(LogLevel.Info, LogLevel.Fatal, new MethodCallTarget("testoutput", (info, objects) => { outputHelper.WriteLine(info.Message); }));

            LogManager.Configuration = config;

            const string cs = "http://serves:8080/;username;password;";

            var serviceSettings = new NexusServiceSettings
            {
                ApiSessions = true,
                IndexHeight = true
            };

            var factory = new LoggerFactory().AddNLog();
            var logger = factory.CreateLogger<NexusServiceFixture>();

            AccountService = new AccountService(logger, new HttpClient(), cs, serviceSettings);
            LedgerService = new LedgerService(logger, new HttpClient(), cs, serviceSettings);
            AssetService = new AssetService(logger, new HttpClient(), cs, serviceSettings);
            TokenService = new TokenService(logger, new HttpClient(), cs, serviceSettings);
            BlockNotify = new BlockNotifyJob(factory.CreateLogger<BlockNotifyJob>(), LedgerService);
        }

        public static NexusUserCredential UserCredential => new NexusUserCredential
        {
            Username = "danielsan",
            Password = "password1",
            Pin = 1234
        };
        
        public static Asset Asset => new Asset
        {
            Name = "danielsan-asset",
            Data = 1.41421356237.ToString(CultureInfo.InvariantCulture)
        };

        public static TokenRegister TokenRegister => new TokenRegister
        {
            Name = "danielsan-token-register",
            Identifier = "42",
            Supply = 3141592
        };

        public static TokenAccount TokenAccount => new TokenAccount
        {
            Name = "danielsan-token-account",
            Identifier = "42"
        };

        public static NexusUserCredential GetRandomUserCredential()
        {
            return new NexusUserCredential
            {
                Username = $"{Guid.NewGuid()}",
                Password = "password1",
                Pin = 1234
            };
        }

        public static Asset GetRandomAsset()
        {
            return new Asset
            {
                Name = $"{Guid.NewGuid()}",
                Data = "hello world"
            };
        }

        public static TokenRegister GetRandomTokenRegister()
        {
            var rnd = new Random((int)DateTime.Now.Ticks);

            return new TokenRegister
            {
                Name = $"{Guid.NewGuid()}",
                Identifier = rnd.Next(100, 999).ToString(),
                Supply = 3141592
            };
        }

        public static TokenAccount GetRandomTokenAccount()
        {
            var rnd = new Random((int)DateTime.Now.Ticks);

            return new TokenAccount()
            {
                Name = $"{Guid.NewGuid()}",
                Identifier = rnd.Next(100, 999).ToString()
            };
        }

        public void Dispose()
        {
            AccountService = null;
            LedgerService = null;
            AssetService = null;
            TokenService = null;
        }
    }
}
