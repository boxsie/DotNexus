using System;
using System.Linq;
using System.Threading.Tasks;
using DotNexus.Account.Models;
using Xunit;
using Xunit.Abstractions;

namespace DotNexus.Tests
{
    public class AccountServiceTests : IClassFixture<NexusServiceFixture>
    {
        private readonly NexusServiceFixture _clientFixture;
        private readonly ITestOutputHelper _output;

        private readonly NexusUser _user = new NexusUser
        {
            Username = "dotnettest",
            Password = "password1",
            Pin = 1234
        };

        public AccountServiceTests(NexusServiceFixture clientFixture, ITestOutputHelper output)
        {
            _clientFixture = clientFixture;
            _output = output;

            _clientFixture.Configure(_output);
        }
        [Fact]
        public async Task AccountService_CreateAccount_ReturnsGenesisTx()
        {
            var genesisTx = await _clientFixture.AccountService.CreateAccountAsync(_user);

            Assert.True(!string.IsNullOrWhiteSpace(genesisTx.Hash));
        }

        [Fact]
        public async Task AccountService_LoginLogout_AccountLogsOut()
        {
            var genesisId = await _clientFixture.AccountService.LoginAsync(_user);

            var logoutGenesis = await _clientFixture.AccountService.LogoutAsync(genesisId.Session);

            Assert.True(logoutGenesis != null && !string.IsNullOrWhiteSpace(logoutGenesis.Genesis));
        }

        [Fact]
        public async Task AccountService_LoginLockUnlockLogout_AccountLocksAndUnlocks()
        {
            var genesisId = await _clientFixture.AccountService.LoginAsync(_user);

            if (genesisId == null || string.IsNullOrWhiteSpace(genesisId.Genesis))
            {
                _output.WriteLine("Account login failed");
                Assert.True(false);
                return;
            }

            if (await _clientFixture.AccountService.LockAsync(genesisId.Session))
            {
                _output.WriteLine("Account is locked");

                if (await _clientFixture.AccountService.UnlockAsync(genesisId.Session, _user.Pin))
                {
                    _output.WriteLine("Account is unlocked");
                    Assert.True(true);
                }
                else
                {
                    _output.WriteLine("Account unlock failed");
                    Assert.True(false);
                }
            }
            else
            {
                _output.WriteLine("Account lock failed");
                Assert.True(false);
            }

            await _clientFixture.AccountService.LogoutAsync(genesisId.Session);
        }

        [Fact]
        public async Task AccountService_GetTransactionsByUsername_ReturnsTransactions()
        {
            var transactions = await _clientFixture.AccountService.GetTransactionsAsync(_user);

            Assert.True(transactions != null);
        }

        [Fact]
        public async Task AccountService_GetTransactionsByGenesis_ReturnsTransactions()
        {
            var user = new NexusUser
            {
                GenesisId = await _clientFixture.AccountService.LoginAsync(_user)
            };

            var transactions = await _clientFixture.AccountService.GetTransactionsAsync(user);

            Assert.True(transactions != null);
        }
    }
}