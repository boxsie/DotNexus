using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DotNexus.Core.Ledger.Models;

namespace DotNexus.App.Models
{
    public class BlockchainBlocksViewModel
    {
        public List<Block> LatestBlocks { get; set; }
    }

    public class HomeConnectViewModel
    {
        public List<string> NexusNodeEndpointIds { get; set; }
    }

    public class ConnectionConnectModel
    {
        [Required]
        public string NodeId { get; set; }
    }
}