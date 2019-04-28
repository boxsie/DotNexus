using Boxsie.Wrapplication.Config.Contracts;

namespace DotNexus.App.Config
{
    public class CookieConfig : ICfg
    {
        public string NodeAuthClaimType { get; set; }
        public string NodeAuthClaimResult { get; set; }
        public string SessionIdKey { get; set; }
        public string GenesisIdKey { get; set; }
        public string UsernameKey { get; set; }
        public string NodeIdClaimType { get; set; }
    }
}