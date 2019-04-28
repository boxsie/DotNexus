using System.Collections.Generic;
using DotNexus.Core.Assets.Models;

namespace DotNexus.App.Models
{
    public class HomeConnectViewModel
    {
        public List<string> NexusNodeEndpointIds { get; set; }
    }

    public class AssetDetailsViewModel
    {
        public AssetInfo AssetInfo { get; set; }
        public List<AssetInfo> AssetHistory { get; set; }
    }

    public class TokenDetailsViewModel
    {

    }
}