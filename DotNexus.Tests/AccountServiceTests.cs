using System;
using System.Linq;
using System.Threading.Tasks;
using DotNexus.Assets.Models;
using Xunit;
using Xunit.Abstractions;

namespace DotNexus.Tests
{
    public class AccountServiceTests : IClassFixture<NexusServiceFixture>
    {
        private readonly NexusServiceFixture _clientFixture;
        private readonly ITestOutputHelper _output;

        public AccountServiceTests(NexusServiceFixture clientFixture, ITestOutputHelper output)
        {
            _clientFixture = clientFixture;
            _output = output;

            _clientFixture.Configure(_output);
        }

        [Fact]
        public async Task AccountService_CreateAccount_ReturnsGenesisTx()
        {
            var genesisId = await _clientFixture.AccountService.CreateAccountAsync(NexusServiceFixture.GetRandomUserCredential());

            Assert.True(!string.IsNullOrWhiteSpace(genesisId?.Genesis));
        }

        [Fact]
        public async Task AccountService_LoginLogout_AccountLogsOut()
        {
            var nexusUser = await _clientFixture.AccountService.LoginAsync(NexusServiceFixture.UserCredential);

            var logoutGenesis = await _clientFixture.AccountService.LogoutAsync(nexusUser.GenesisId.Session);

            Assert.True(logoutGenesis != null && !string.IsNullOrWhiteSpace(logoutGenesis.Genesis));
        }

        [Fact]
        public async Task AccountService_LoginLogout_AccountLogsInAndLogsOut()
        {
            var nexusUser = await _clientFixture.AccountService.LoginAsync(NexusServiceFixture.UserCredential);

            if (nexusUser?.GenesisId == null || string.IsNullOrWhiteSpace(nexusUser.GenesisId.Genesis))
            {
                _output.WriteLine("Account login failed");
                Assert.True(false);
                return;
            }

            await _clientFixture.AccountService.LogoutAsync(nexusUser.GenesisId.Session);
            Assert.True(true);
        }

        [Fact]
        public async Task AccountService_GetTransactionsByUsername_ReturnsTransactions()
        {
            var transactions = await _clientFixture.AccountService.GetTransactionsAsync(NexusServiceFixture.UserCredential.Username);

            Assert.True(transactions != null);
        }

        [Fact]
        public async Task AccountService_GetTransactionsByGenesis_ReturnsTransactions()
        {
            var user = await _clientFixture.AccountService.LoginAsync(NexusServiceFixture.UserCredential);

            var transactions = await _clientFixture.AccountService.GetTransactionsAsync(user.GenesisId);

            Assert.True(transactions != null);
        }
    }
}