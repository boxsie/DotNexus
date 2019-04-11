using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using DotNexus.Account.Models;
using DotNexus.Assets.Models;
using DotNexus.Core;
using DotNexus.Core.Enums;
using DotNexus.Ledger.Models;
using NLog;

namespace DotNexus.Assets
{
    public class AssetService : NexusService
    {
        public AssetService(ILogger log, HttpClient client, string connectionString, NexusServiceSettings serviceSettings) 
            : base(log, client, connectionString, serviceSettings) { }

        public async Task<Asset> CreateAssetAsync(Asset asset, NexusUser user, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            user.Validate(UserValidationMode.Authenticate);
            asset.Validate();

            var request = new NexusRequest(new Dictionary<string, string>
            {
                {"pin", user.Pin.ToString()},
                {"session", user.GenesisId.Session},
                {"data", asset.Data},
                {"name", asset.Name}
            });

            var response = await GetAsync<NexusCreationResponse>("assets/create", request, token);

            if (string.IsNullOrWhiteSpace(response?.Address))
                throw new InvalidOperationException($"{asset.Name} creation failed");

            asset.Address = response.Address;
            asset.TxId = response.TxId;

            return asset;
        }

        public async Task<AssetInfo> GetAssetAsync(Asset asset, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            asset.Validate();

            var assetKeyVal = asset.GetKeyVal();

            var request = new NexusRequest(new Dictionary<string, string> {{assetKeyVal.Item1, assetKeyVal.Item2}});

            var assetInfo = await GetAsync<AssetInfo>("assets/get", request, token);

            if (assetInfo == null)
                throw new InvalidOperationException($"{asset.Name} retrieval failed");

            return assetInfo;
        }

        public async Task<Asset> TransferAssetAsync(Asset asset, NexusUser fromUser, NexusUser toUser, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            fromUser.Validate(UserValidationMode.Authenticate);
            toUser.Validate(UserValidationMode.Lookup);
            asset.Validate();

            var toUserKeyVal = toUser.GetLookupKeyVal("destination");
            var assetKeyVal = asset.GetKeyVal();

            var request = new NexusRequest(new Dictionary<string, string>
            {
                {"pin", fromUser.Pin.ToString()},
                {"session", fromUser.GenesisId.Session},
                {toUserKeyVal.Item1, toUserKeyVal.Item2},
                {assetKeyVal.Item1, assetKeyVal.Item2}
            });

            var newId = await GetAsync<Asset>("assets/transfer", request, token);

            if (newId == null)
                throw new InvalidOperationException($"{asset.Name} transfer from {fromUser.Username} to {toUserKeyVal.Item2} failed");

            return asset;
        }

        public async Task<object> TokeniseAsset(Asset asset, Token nexusToken, NexusUser user, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            user.Validate(UserValidationMode.Authenticate);
            asset.Validate();
            nexusToken.Validate();

            var assetKeyVal = asset.GetKeyVal("asset_address", "asset_name");
            var tokenKeyVal = nexusToken.GetKeyVal("token_address", "token_name");

            var request = new NexusRequest(new Dictionary<string, string>
            {
                {"pin", user.Pin.ToString()},
                {"session", user.GenesisId.Session},
                {assetKeyVal.Item1, assetKeyVal.Item2},
                {tokenKeyVal.Item1, tokenKeyVal.Item2}
            });

            return await GetAsync<Asset>("assets/tokenize", request, token);
        }

        public async Task<IEnumerable<AssetInfo>> GetAssetHistoryAsync(Asset asset, CancellationToken token = default)
        {
            asset.Validate();

            var assetKeyVal = asset.GetKeyVal();
            
            var request = new NexusRequest(new Dictionary<string, string> { { assetKeyVal.Item1, assetKeyVal.Item2 } });

            var assetHistory = await GetAsync<IEnumerable<AssetInfo>>("assets/history", request, token);

            if (assetHistory == null)
                throw new InvalidOperationException($"Get asset {assetKeyVal.Item2} history failed");

            return assetHistory;
        }
    }
}