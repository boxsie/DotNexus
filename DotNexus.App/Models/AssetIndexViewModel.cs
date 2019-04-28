using System.Collections.Generic;
using DotNexus.Core.Assets.Models;
using DotNexus.Core.Tokens.Models;

namespace DotNexus.App.Models
{
    public class AssetIndexViewModel
    {
        public List<Asset> UserAssets { get; set; }
    }

    public class TokenIndexViewModel
    {
        public List<Token> UserTokens { get; set; }
    }
}