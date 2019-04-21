using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DotNexus.Core.Accounts.Models;
using DotNexus.Core.Assets.Models;
using DotNexus.Core.Nexus;
using DotNexus.Core.Tokens.Models;
using Microsoft.Extensions.Logging;

namespace DotNexus.Core.Assets
{
    public class AssetService : NexusService
    {
        public AssetService(INexusClient client, NexusSettings settings, ILogger log = null)
            : base(client, settings, log) { }

        public async Task<Asset> CreateAssetAsync(Asset asset, NexusUser user, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            user.Validate();
            asset.Validate();

            var request = new NexusRequest(new Dictionary<string, string>
            {
                {"pin", user.Pin.ToString()},
                {"session", user.GenesisId.Session},
                {"data", asset.Data},
                {"name", asset.Name}
            });

            var response = await PostAsync<NexusCreationResponse>("assets/create", request, token);

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

            var assetInfo = await PostAsync<AssetInfo>("assets/get", request, token);

            if (assetInfo == null)
                throw new InvalidOperationException($"{asset.Name} retrieval failed");

            return assetInfo;
        }

        public async Task<Asset> TransferAssetAsync(Asset asset, NexusUser fromUser, GenesisId toUserGenesis, CancellationToken token = default)
        {
            if (string.IsNullOrWhiteSpace(toUserGenesis?.Genesis))
                throw new ArgumentException("Genesis is required");

            return await TransferAssetAsync(asset, fromUser, ("destination", toUserGenesis.Genesis), token);
        }

        public async Task<Asset> TransferAssetAsync(Asset asset, NexusUser fromUser, string toUsername, CancellationToken token = default)
        {
            if (string.IsNullOrWhiteSpace(toUsername))
                throw new ArgumentException("Username is required");

            return await TransferAssetAsync(asset, fromUser, ("username", toUsername), token);
        }

        public async Task<object> TokeniseAsset(Asset asset, Token nexusToken, NexusUser user, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            user.Validate();
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

            return await PostAsync<Asset>("assets/tokenize", request, token);
        }

        public async Task<IEnumerable<AssetInfo>> GetAssetHistoryAsync(Asset asset, CancellationToken token = default)
        {
            asset.Validate();

            var assetKeyVal = asset.GetKeyVal();
            
            var request = new NexusRequest(new Dictionary<string, string> { { assetKeyVal.Item1, assetKeyVal.Item2 } });

            var assetHistory = await PostAsync<IEnumerable<AssetInfo>>("assets/history", request, token);

            if (assetHistory == null)
                throw new InvalidOperationException($"Get asset {assetKeyVal.Item2} history failed");

            return assetHistory;
        }

        private async Task<Asset> TransferAssetAsync(Asset asset, NexusUser fromUser, (string, string) toUserKeyVal,
            CancellationToken token = default)
        {
            fromUser.Validate();
            asset.Validate();

            var assetKeyVal = asset.GetKeyVal();

            var request = new NexusRequest(new Dictionary<string, string>
            {
                {"pin", fromUser.Pin.ToString()},
                {"session", fromUser.GenesisId.Session},
                {toUserKeyVal.Item1, toUserKeyVal.Item2},
                {assetKeyVal.Item1, assetKeyVal.Item2}
            });

            var newId = await PostAsync<Asset>("assets/transfer", request, token);

            if (newId == null)
                throw new InvalidOperationException($"{asset.Name} transfer from {fromUser.GenesisId.Genesis} to {toUserKeyVal.Item2} failed");

            return asset;
        }
    }
}