using System;
using System.Linq;
using System.Threading.Tasks;
using DotNexus.Assets.Models;
using Xunit;
using Xunit.Abstractions;

namespace DotNexus.Tests
{
    public class InitTests : IClassFixture<NexusServiceFixture>
    {
        private readonly NexusServiceFixture _clientFixture;
        private readonly ITestOutputHelper _output;

        public InitTests(NexusServiceFixture clientFixture, ITestOutputHelper output)
        {
            _clientFixture = clientFixture;
            _output = output;

            _clientFixture.Configure(_output);

            Task.Run(() => _clientFixture.AccountService.CreateAccountAsync(NexusServiceFixture.UserCredential));
        }

        [Fact]
        public async Task InitTest_CreateFixedAccountAssetAndTokens_AllCreated()
        {
            var user = await _clientFixture.AccountService.LoginAsync(NexusServiceFixture.UserCredential);
            await Task.Delay(TimeSpan.FromSeconds(5));
            var asset = await _clientFixture.AssetService.CreateAssetAsync(NexusServiceFixture.Asset, user);
            await Task.Delay(TimeSpan.FromSeconds(5));
            var rToken = await _clientFixture.TokenService.CreateTokenAsync(NexusServiceFixture.TokenRegister, user);
            await Task.Delay(TimeSpan.FromSeconds(5));
            var aToken = await _clientFixture.TokenService.CreateTokenAsync(NexusServiceFixture.TokenAccount, user);

            Assert.True(true);
        }
    }
}