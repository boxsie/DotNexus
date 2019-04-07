using System;
using System.Linq;
using System.Threading.Tasks;
using DotNexus.Account.Models;
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
            var genesisTx = await _clientFixture.AccountService.CreateAccountAsync(_clientFixture.GetUser());

            Assert.True(!string.IsNullOrWhiteSpace(genesisTx.Hash));
        }

        [Fact]
        public async Task AccountService_LoginLogout_AccountLogsOut()
        {
            var nexusUser = await _clientFixture.AccountService.LoginAsync(_clientFixture.GetUser());

            var logoutGenesis = await _clientFixture.AccountService.LogoutAsync(nexusUser.GenesisId.Session);

            Assert.True(logoutGenesis != null && !string.IsNullOrWhiteSpace(logoutGenesis.Genesis));
        }

        [Fact]
        public async Task AccountService_LoginLogout_AccountLogsInAndLogsOut()
        {
            var nexusUser = await _clientFixture.AccountService.LoginAsync(_clientFixture.GetUser());

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
            var transactions = await _clientFixture.AccountService.GetTransactionsAsync(_clientFixture.GetUser());

            Assert.True(transactions != null);
        }

        [Fact]
        public async Task AccountService_GetTransactionsByGenesis_ReturnsTransactions()
        {
            var user = await _clientFixture.AccountService.LoginAsync(_clientFixture.GetUser());

            var transactions = await _clientFixture.AccountService.GetTransactionsAsync(user);

            Assert.True(transactions != null);
        }
    }
}