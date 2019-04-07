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
            var user = await _clientFixture.AccountService.LoginAsync(_clientFixture.GetUser());

            var assetId = await _clientFixture.AssetService.CreateAssetAsync(_clientFixture.GetAsset(), user);

            Assert.True(!string.IsNullOrWhiteSpace(assetId?.Address));
        }

        [Fact]
        public async Task AssetService_CreateAssetRetrieveAsset_ReturnsAsset()
        {
            var user = await _clientFixture.AccountService.LoginAsync(_clientFixture.GetUser());

            var assetId = await _clientFixture.AssetService.CreateAssetAsync(_clientFixture.GetAsset(), user);

            var asset = await _clientFixture.AssetService.GetAssetAsync(assetId);

            Assert.True(!string.IsNullOrWhiteSpace(asset.Owner));
        }
    }
}