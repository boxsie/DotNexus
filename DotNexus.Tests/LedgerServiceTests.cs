using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace DotNexus.Tests
{
    public class LedgerServiceTests : IClassFixture<NexusServiceFixture>
    {
        private readonly NexusServiceFixture _clientFixture;

        public LedgerServiceTests(NexusServiceFixture clientFixture, ITestOutputHelper output)
        {
            _clientFixture = clientFixture;

            _clientFixture.Configure(output);
        }

        [Fact]
        public async Task LedgerService_GetBlockWithGenesisHash_ReturnsGenesisBlock()
        {
            var hash = await _clientFixture.LedgerService.GetBlockHashAsync(1);

            var block = await _clientFixture.LedgerService.GetBlockAsync(hash);
            
            Assert.True(block?.Hash == hash && block?.Height == 1);
        }

        [Fact]
        public async Task LedgerService_GetBlockWithGenesisHeight_ReturnsBlock()
        {
            var block = await _clientFixture.LedgerService.GetBlockAsync(1);
            
            Assert.True(block?.Hash != null);
        }

        [Fact]
        public async Task LedgerService_GetTwentyBlocksFromGenesis_ReturnsTwentyBlocks()
        {
            var hash = await _clientFixture.LedgerService.GetBlockHashAsync(1);

            var blocks = (await _clientFixture.LedgerService.GetBlocksAsync(hash, 20)).ToList();
            
            Assert.True(blocks != null && blocks.Count == 20);
        }

        [Fact]
        public async Task LedgerService_GetGenesisTransaction_ReturnsTransaction()
        {
            var block = await _clientFixture.LedgerService.GetBlockAsync(1);

            var tx = await _clientFixture.LedgerService.GetTransactionAsync(block.Tx[0].Hash);
            
            Assert.True(tx != null && tx.Hash == block.Tx[0].Hash);
        }

        [Fact]
        public async Task LedgerService_StartBlockNotifySendTransaction_NotificationOfNewBlocks()
        {
            var token = new CancellationTokenSource();

            var notified = false;

            _clientFixture.BlockNotify.Subscribe(async block =>
            {
                notified = true;
                await _clientFixture.BlockNotify.StopAsync(token.Token);
                Assert.True(true);
            });

            await _clientFixture.BlockNotify.StartAsync(token.Token);

            var user = await _clientFixture.AccountService.LoginAsync(NexusServiceFixture.UserCredential, token.Token);
            await _clientFixture.AssetService.CreateAssetAsync(NexusServiceFixture.GetRandomAsset(), user, token.Token);

            while (!notified)
                await Task.Delay(1, token.Token);
        }
    }
}