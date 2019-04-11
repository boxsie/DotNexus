using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
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
        public AccountService(ILogger log, HttpClient client, string connectionString, NexusServiceSettings serviceSettings) 
            : base(log, client, connectionString, serviceSettings) { }

        public async Task<NexusUser> CreateAccountAsync(NexusUser user, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            user.Validate(UserValidationMode.Create);

            var request = new NexusRequest(new Dictionary<string, string>
            {
                {"username", user.Username},
                {"password", user.Password},
                {"pin", user.Pin.ToString()}
            });
            
            var tx = await GetAsync<Tx>("accounts/create", request, token);

            if (string.IsNullOrWhiteSpace(tx?.Genesis))
                throw new InvalidOperationException($"Create account {user.Username} failed");

            user.GenesisId = new GenesisId {Genesis = tx.Genesis};

            return user;
        }

        public async Task<NexusUser> LoginAsync(NexusUser user, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            user.Validate(UserValidationMode.Create);

            var param = new Dictionary<string, string>
            {
                {"username", user.Username},
                {"password", user.Password}
            };

            if (user.Pin > 0)
                param.Add("pin", user.Pin.ToString());

            var genesisId = await GetAsync<GenesisId>("accounts/login", new NexusRequest(param), token);

            user.GenesisId = genesisId ?? throw new InvalidOperationException($"{user.Username} login failed");

            return user;
        }

        public async Task<GenesisId> LogoutAsync(string sessionId = null, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            var request = !string.IsNullOrWhiteSpace(sessionId)
                ? new NexusRequest(new Dictionary<string, string> {{"session", sessionId}})
                : null;

            var id = await GetAsync<GenesisId>("accounts/logout", request, token);

            if (string.IsNullOrWhiteSpace(id?.Genesis))
                throw new InvalidOperationException("Logout failed");

            return id;
        }

        public async Task<bool> LockAsync(string sessionId, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            if (ServiceSettings.ApiSessions)
                throw new InvalidOperationException("Cannot lock with API sessions enabled");

            var request = new NexusRequest(new Dictionary<string, string> {{"session", sessionId}} );

            return await GetAsync<bool>("accounts/lock", request, token);
        }

        public async Task<bool> UnlockAsync(int pin, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            if (ServiceSettings.ApiSessions)
                throw new InvalidOperationException("Cannot unlock with API sessions enabled");

            if (pin.ToString().Length < 4)
                throw new ArgumentException("A valid PIN is required to unlock the account");

            var request = new NexusRequest(new Dictionary<string, string> {{"pin", pin.ToString()}});

            return await GetAsync<bool>("accounts/unlock", request, token);
        }

        public async Task<IEnumerable<Tx>> GetTransactionsAsync(NexusUser nexusUser, 
            TxVerbosity txVerbosity = TxVerbosity.PubKeySign, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            nexusUser.Validate(UserValidationMode.Lookup);

            var userKeyVal = nexusUser.GetLookupKeyVal();

            var request = new NexusRequest(new Dictionary<string, string>
            {
                {userKeyVal.Item1, userKeyVal.Item2},
                {"verbose", ((int)txVerbosity).ToString()}
            });

            var txs = await GetAsync<IEnumerable<Tx>>("accounts/transactions", request, token);

            return txs ?? new List<Tx>();
        }
    }
}