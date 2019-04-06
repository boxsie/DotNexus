using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace DotNexus.Tests
{
    public class LedgerServiceTests : IClassFixture<NexusServiceFixture>
    {
        private readonly NexusServiceFixture _clientFixture;
        private readonly ITestOutputHelper _output;

        private const string GenesisHash =
            "c8b01fc7f3fa20237a9440e0645fea8876ca6fa2367f926e1ce262264e536bfc77865f54ad5eade99295f" +
            "e014fb6f78fa6a6a9356bc732a1ca50952af368a0b6e89bec3f51e9afb42627220f70d9a68f6b6bc74742" +
            "b3bb7c669ccc1e05e34142e0ea3b399b405598302ffafa3f94cfa53b1536392ac1b4acad4176cfdea3fa0e";

        private const string GenesisTxHash = 
            "f4c08137bea170449e1be91409c7c227a62f7e3721e02f5dc5e9250e3c91dbb8" +
            "092c32bf2a15b7a5f3f760d2714d07e0470a2c9f0bb1d117a3c95b9d091a295f";

        public LedgerServiceTests(NexusServiceFixture clientFixture, ITestOutputHelper output)
        {
            _clientFixture = clientFixture;
            _output = output;

            _clientFixture.Configure(_output);
        }

        [Fact]
        public async Task LedgerService_GetBlockWithGenesisHash_ReturnsGenesisBlock()
        {
            var block = await _clientFixture.LedgerService.GetBlockAsync(GenesisHash);
            
            Assert.True(block.Hash == GenesisHash && block.Height == 1);
        }

        [Fact]
        public async Task LedgerService_GetBlockWithGenesisHeight_ReturnsGenesisHash()
        {
            var block = await _clientFixture.LedgerService.GetBlockAsync(1);
            
            Assert.True(block.Hash == GenesisHash);
        }

        [Fact]
        public async Task LedgerService_GetTenBlocksFromGenesis_ReturnsTenBlocks()
        {
            var blocks = (await _clientFixture.LedgerService.GetBlocksAsync(GenesisHash, 10)).ToList();
            
            Assert.True(blocks != null && blocks.Count == 10);
        }

        [Fact]
        public async Task LedgerService_GetGenesisTransaction_ReturnsTransaction()
        {
            var tx = await _clientFixture.LedgerService.GetTransactionAsync(GenesisTxHash);
            
            Assert.True(tx != null && tx.Hash == GenesisTxHash);
        }
    }
}