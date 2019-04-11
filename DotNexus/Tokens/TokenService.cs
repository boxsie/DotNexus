using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DotNexus.Account.Models;
using DotNexus.Assets.Models;
using DotNexus.Core;
using DotNexus.Core.Enums;
using NLog;

namespace DotNexus.Tokens
{
    public class TokenService : NexusService
    {
        public TokenService(ILogger log, HttpClient client, string connectionString, NexusServiceSettings serviceSettings) 
            : base(log, client, connectionString, serviceSettings) { }

        public async Task<T> CreateTokenAsync<T>(T token, NexusUser user, CancellationToken cToken = default) where T : Token
        {
            cToken.ThrowIfCancellationRequested();

            user.Validate(UserValidationMode.Authenticate);
            token.Validate();

            var param = new Dictionary<string, string>
            {
                {"pin", user.Pin.ToString()},
                {"session", user.GenesisId.Session},
                {"identifier", token.Identifier},
                {"name", token.Name},
                {"type", token.Type}
            };

            if (token is TokenRegister register)
                param.Add("supply", register.Supply.ToString(CultureInfo.InvariantCulture));

            var response = await GetAsync<NexusCreationResponse>("tokens/create", new NexusRequest(param), cToken);

            if (string.IsNullOrWhiteSpace(response?.Address))
                throw new InvalidOperationException($"{token.Name} creation failed");

            token.Address = response.Address;
            token.Tx = response.TxId;

            return token;
        }

        public async Task<TokenInfo> GetTokenInfo(Token token, CancellationToken cToken = default)
        {
            cToken.ThrowIfCancellationRequested();

            token.Validate();

            var keyVal = token.GetKeyVal();

            var request = new NexusRequest(new Dictionary<string, string>
            {
                {"identifier", token.Identifier},
                {keyVal.Item1, keyVal.Item2},
                {"type", token.Type}
            });

            var tokenInfo = await GetAsync<TokenInfo>("tokens/get", request, cToken);

            if (tokenInfo == null)
                throw new InvalidOperationException($"{token.Name} get failed");

            return tokenInfo;
        }
    }
}