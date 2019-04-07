using System;
using System.Net.Http;
using System.Threading.Tasks;
using DotNexus.Account;
using DotNexus.Account.Models;
using DotNexus.Assets;
using DotNexus.Assets.Models;
using DotNexus.Core;
using DotNexus.Ledger;
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

        public const bool IsApiSession = true;

        public void Configure(ITestOutputHelper outputHelper)
        {
            var config = new LoggingConfiguration();
            config.AddRule(LogLevel.Info, LogLevel.Fatal,
                new MethodCallTarget("testoutput", (info, objects) => { outputHelper.WriteLine(info.Message); }));

            LogManager.Configuration = config;

            const string cs = "http://serves:8080/;username;password;";

            AccountService = new AccountService(LogManager.GetCurrentClassLogger(), new HttpClient(), cs);
            LedgerService = new LedgerService(LogManager.GetCurrentClassLogger(), new HttpClient(), cs);
            AssetService = new AssetService(LogManager.GetCurrentClassLogger(), new HttpClient(), cs);
        }

        public NexusUser GetUser()
        {
            return new NexusUser
            {
                Username = "dotnexus",
                Password = "password1",
                Pin = 1234
            };
        }

        public Asset GetAsset()
        {
            return new Asset
            {
                Name = $"{Guid.NewGuid()}",
                Data = "hello world"
            };
        }

        public void Dispose()
        {
            AccountService.LogoutAsync().GetAwaiter().GetResult();

            AccountService = null;
            LedgerService = null;
        }
    }
}
