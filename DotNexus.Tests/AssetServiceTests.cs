using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace DotNexus.Tests
{
    public class AssetServiceTests : IClassFixture<NexusServiceFixture>
    {
        private readonly NexusServiceFixture _clientFixture;
        
        public AssetServiceTests(NexusServiceFixture clientFixture, ITestOutputHelper output)
        {
            _clientFixture = clientFixture;

            _clientFixture.Configure(output);
        }

        [Fact]
        public async Task AssetService_CreateAsset_ReturnsAssetId()
        {
            var user = await _clientFixture.AccountService.LoginAsync(NexusServiceFixture.User);

            var assetId = await _clientFixture.AssetService.CreateAssetAsync(NexusServiceFixture.GetRandomAsset(), user);

            Assert.True(!string.IsNullOrWhiteSpace(assetId?.Address));
        }

        [Fact]
        public async Task AssetService_CreateAssetRetrieveAsset_ReturnsAsset()
        {
            var user = await _clientFixture.AccountService.LoginAsync(NexusServiceFixture.User);
            
            var assetId = await _clientFixture.AssetService.CreateAssetAsync(NexusServiceFixture.GetRandomAsset(), user);
            
            await Task.Delay(TimeSpan.FromSeconds(5));

            var asset = await _clientFixture.AssetService.GetAssetAsync(assetId);

            Assert.True(!string.IsNullOrWhiteSpace(asset.Owner));
        }

        [Fact]
        public async Task AssetService_CreateAssetCreateUserTransferAsset_AssetHistoryReflectsTransfer()
        {
            var fromUser = await _clientFixture.AccountService.LoginAsync(NexusServiceFixture.User);
            var toUser = await _clientFixture.AccountService.LoginAsync(await _clientFixture.AccountService.CreateAccountAsync(NexusServiceFixture.GetRandomUser()));
            var asset = await _clientFixture.AssetService.CreateAssetAsync(NexusServiceFixture.GetRandomAsset(), fromUser);

            await Task.Delay(TimeSpan.FromSeconds(5));

            await _clientFixture.AssetService.TransferAssetAsync(asset, fromUser, toUser);
            
            await Task.Delay(TimeSpan.FromSeconds(5));
            
            var history = await _clientFixture.AssetService.GetAssetHistoryAsync(asset);

            Assert.True(history.Count() == 2);
        }
    }
}