using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using DotNexus.Account;
using DotNexus.Account.Models;
using DotNexus.Assets;
using DotNexus.Assets.Models;
using DotNexus.Core;
using DotNexus.Ledger;
using DotNexus.Tokens;
using Newtonsoft.Json;
using NLog;
using NLog.Config;
using NLog.Targets;
using Xunit.Abstractions;

namespace DotNexus.Tests
{
    public class NexusServiceFixture : IDisposable
    {
        public LedgerService LedgerService { get; private set; }
        public AccountService AccountService { get; private set; }
        public AssetService AssetService { get; private set; }
        public TokenService TokenService { get; private set; }

        public const bool IsApiSession = true;

        public void Configure(ITestOutputHelper outputHelper)
        {
            var config = new LoggingConfiguration();
            config.AddRule(LogLevel.Info, LogLevel.Fatal,
                new MethodCallTarget("testoutput", (info, objects) => { outputHelper.WriteLine(info.Message); }));

            LogManager.Configuration = config;

            const string cs = "http://serves:8080/;username;password;";

            var serviceSettings = new NexusServiceSettings
            {
                ApiSessions = true,
                IndexHeight = true
            };

            AccountService = new AccountService(LogManager.GetCurrentClassLogger(), new HttpClient(), cs, serviceSettings);
            LedgerService = new LedgerService(LogManager.GetCurrentClassLogger(), new HttpClient(), cs, serviceSettings);
            AssetService = new AssetService(LogManager.GetCurrentClassLogger(), new HttpClient(), cs, serviceSettings);
            TokenService = new TokenService(LogManager.GetCurrentClassLogger(), new HttpClient(), cs, serviceSettings);
        }

        public static NexusUser User => new NexusUser
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

        public static NexusUser GetRandomUser()
        {
            return new NexusUser
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
