using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using DotNexus.Accounts;
using DotNexus.Accounts.Models;
using DotNexus.Assets;
using DotNexus.Assets.Models;
using DotNexus.Core.Nexus;
using DotNexus.Ledger;
using DotNexus.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using Xunit.Abstractions;
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
            var logConfig = new LoggingConfiguration();
            logConfig.AddRule(LogLevel.Info, LogLevel.Fatal, new MethodCallTarget("testoutput", (info, objects) => { outputHelper.WriteLine(info.Message); }));
            LogManager.Configuration = logConfig;

            var factory = new LoggerFactory().AddNLog();

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            AccountService = new AccountService(factory.CreateLogger<AccountService>(), new NexusClient(factory.CreateLogger<NexusClient>(), new HttpClient(), config), config);
            LedgerService = new LedgerService(factory.CreateLogger<LedgerService>(), new NexusClient(factory.CreateLogger<NexusClient>(), new HttpClient(), config), config);
            AssetService = new AssetService(factory.CreateLogger<AssetService>(), new NexusClient(factory.CreateLogger<NexusClient>(), new HttpClient(), config), config);
            TokenService = new TokenService(factory.CreateLogger<TokenService>(), new NexusClient(factory.CreateLogger<NexusClient>(), new HttpClient(), config), config);
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
