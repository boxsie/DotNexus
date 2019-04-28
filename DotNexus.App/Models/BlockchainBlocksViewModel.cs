using System.Collections.Generic;
using DotNexus.Core.Ledger.Models;

namespace DotNexus.App.Models
{
    public class BlockchainBlocksViewModel
    {
        public List<Block> LatestBlocks { get; set; }
    }
}