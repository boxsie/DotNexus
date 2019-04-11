using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotNexus.Assets.Models;
using Xunit;
using Xunit.Abstractions;

namespace DotNexus.Tests
{
    public class TokenServiceTests : IClassFixture<NexusServiceFixture>
    {
        private readonly NexusServiceFixture _clientFixture;
        
        public TokenServiceTests(NexusServiceFixture clientFixture, ITestOutputHelper output)
        {
            _clientFixture = clientFixture;

            _clientFixture.Configure(output);
        }

        [Fact]
        public async Task TokenService_CreateTokenRegisterAndAccount_ReturnsTokenInfo()
        {
            var user = await _clientFixture.AccountService.LoginAsync(NexusServiceFixture.User);

            var rtoken = NexusServiceFixture.GetRandomTokenRegister();
            var aToken = NexusServiceFixture.GetRandomTokenAccount();
            aToken.Identifier = rtoken.Identifier;

            var tokenRegister = await _clientFixture.TokenService.CreateTokenAsync(NexusServiceFixture.TokenRegister, user);

            await Task.Delay(TimeSpan.FromSeconds(5));
            
            var tokenAccount = await _clientFixture.TokenService.CreateTokenAsync(NexusServiceFixture.TokenAccount, user);

            await Task.Delay(TimeSpan.FromSeconds(5));

            var tokenRegisterInfo = await _clientFixture.TokenService.GetTokenInfo(tokenRegister);
            var tokenAccountInfo = await _clientFixture.TokenService.GetTokenInfo(tokenAccount);

            Assert.True(tokenRegisterInfo != null && tokenRegisterInfo.Identifier == rtoken.Identifier);
            Assert.True(tokenAccountInfo != null && tokenAccountInfo.Identifier == aToken.Identifier);
        }
    }
}