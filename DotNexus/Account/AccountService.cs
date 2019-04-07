using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using DotNexus.Account.Models;
using DotNexus.Core;
using DotNexus.Core.Enums;
using DotNexus.Ledger;
using DotNexus.Ledger.Models;
using NLog;

namespace DotNexus.Account
{
    public class AccountService : NexusService
    {
        public AccountService(ILogger log, HttpClient client, string connectionString) 
            : base(log, client, connectionString) { }

        public async Task<Tx> CreateAccountAsync(NexusUser user, CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();

            if (user == null || string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
                throw new Exception("Username and/or password is required to create an account");

            if (user.Pin.ToString().Length < 4)
                throw new Exception("PIN must be greater than 4 digits to create an account");

            var request = new NexusRequest(new Dictionary<string, string>
            {
                {"username", user.Username},
                {"password", user.Password},
                {"pin", user.Pin.ToString()}
            });

            return await GetAsync<Tx>("accounts/create", request, token);
        }

        public async Task<NexusUser> LoginAsync(NexusUser user, CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();

            if (user == null || string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
                throw new Exception("Username and/or password is required to login");

            var param = new Dictionary<string, string>
            {
                {"username", user.Username},
                { "password", user.Password}
            };

            if (user.Pin > 0)
                param.Add("pin", user.Pin.ToString());

            var genesisId = await GetAsync<GenesisId>("accounts/login", new NexusRequest(param), token);

            user.GenesisId = genesisId ?? throw new Exception($"{user.Username} login failed");

            return user;
        }

        public async Task<GenesisId> LogoutAsync(string sessionId = null, CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();

            NexusRequest request = null;

            if (!string.IsNullOrWhiteSpace(sessionId))
            {
                request = new NexusRequest(new Dictionary<string, string>
                {
                    {"session", sessionId}
                });
            }

            return await GetAsync<GenesisId>("accounts/logout", request, token);
        }

        public async Task<bool> LockAsync(string sessionId, CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(sessionId))
                throw new Exception("A session ID is required to lock the account");

            var request = new NexusRequest(new Dictionary<string, string>
            {
                {"session", sessionId}
            });

            return await GetAsync<bool>("accounts/lock", request, token);
        }

        public async Task<bool> UnlockAsync(string sessionId, int pin, CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(sessionId))
                throw new Exception("A session ID is required to unlock the account");

            if (pin.ToString().Length < 4)
                throw new Exception("A valid PIN is required to unlock the account");

            var request = new NexusRequest(new Dictionary<string, string>
            {
                {"session", sessionId},
                {"pin", pin.ToString()}
            });

            return await GetAsync<bool>("accounts/unlock", request, token);
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsAsync(NexusUser nexusUser, 
            TxVerbosity txVerbosity = TxVerbosity.PubKeySign, CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(nexusUser.Username) && string.IsNullOrWhiteSpace(nexusUser?.GenesisId.Genesis))
                throw new Exception("A valid genesis ID or username is required to retrieve transactions");

            var useGenesis = nexusUser.GenesisId != null;
            var key = useGenesis ? "genesis" : "username";
            var val = useGenesis ? nexusUser.GenesisId.Genesis : nexusUser.Username;

            var request = new NexusRequest(new Dictionary<string, string>
            {
                {key, val},
                {"verbose", ((int)txVerbosity).ToString()}
            });

            return await GetAsync<IEnumerable<Transaction>>("accounts/transactions", request, token) ?? new List<Transaction>();
        }
    }
}