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
        public AssetService(ILogger log, HttpClient client, string connectionString) 
            : base(log, client, connectionString) { }

        public async Task<AssetId> CreateAssetAsync(Asset asset, NexusUser user, CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();

            if (user == null || asset == null)
                throw new Exception("User and/or asset are required to create an asset");

            if (string.IsNullOrWhiteSpace(asset.Name) || string.IsNullOrWhiteSpace(asset.Data))
                throw new Exception("Name and/or data is required to create an asset");

            if (user.Pin.ToString().Length < 4)
                throw new Exception("PIN must be greater than 4 digits to create an account");

            if (string.IsNullOrWhiteSpace(user.GenesisId?.Session))
                throw new Exception("A session key is required to create an asset");

            var request = new NexusRequest(new Dictionary<string, string>
            {
                {"pin", user.Pin.ToString()},
                {"session", user.GenesisId.Session},
                {"data", asset.Data},
                {"name", asset.Name}
            });

            var assetId = await GetAsync<AssetId>("assets/create", request, token);

            if (assetId == null)
                throw new Exception($"{asset.Name} creation failed");

            assetId.Name = asset.Name;

            return assetId;
        }

        public async Task<AssetInfo> GetAssetAsync(AssetId assetId, CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();

            if (assetId == null)
                throw new Exception("Asset ID is required to get an asset");

            if (string.IsNullOrWhiteSpace(assetId.Address) && string.IsNullOrWhiteSpace(assetId.Name))
                throw new Exception("Name or address is required to get an asset");

            var useAddress = assetId.Address != null;
            var key = useAddress ? "address" : "name";
            var val = useAddress ? assetId.Address : assetId.Name;

            var request = new NexusRequest(new Dictionary<string, string> {{key, val}});

            return await GetAsync<AssetInfo>("assets/get", request, token);
        }
    }
}