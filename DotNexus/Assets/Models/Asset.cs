namespace DotNexus.Assets.Models
{
    public class NexusCreationResponse
    {
        public string TxId { get; set; }
        public string Address { get; set; }
    }

    public class TokenInfo
    {
        public string Version { get; set; }
        public string Identifier { get; set; }
        public double Balance { get; set; }
        public double MaxSupply { get; set; }
        public double CurrentSupply { get; set; }
    }

    public abstract class Token
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Identifier { get; set; }
        public string Tx { get; set; }

        public abstract string Type { get; }

        public (string, string) GetKeyVal(string addressKey = nameof(Address), string nameKey = nameof(Name))
        {
            var useAddress = !string.IsNullOrWhiteSpace(Address);
            var key = useAddress ? addressKey.ToLower() : nameKey.ToLower();
            var val = useAddress ? Address : Name;

            return (key, val);
        }
    }

    public class TokenRegister : Token
    {
        public override string Type => "token";

        public double Supply { get; set; }
    }

    public class TokenAccount : Token
    {
        public override string Type => "account";
    }

    public class Asset
    {
        public string Address { get; set; }
        public string Name { get; set; }
        public string TxId { get; set; }
        public string Data { get; set; }

        public (string, string) GetKeyVal(string addressKey = nameof(Address), string nameKey = nameof(Name))
        {
            var useAddress = !string.IsNullOrWhiteSpace(Address);
            var key = useAddress ? addressKey.ToLower() : nameKey.ToLower();
            var val = useAddress ? Address : Name;

            return (key, val);
        }
    }
}