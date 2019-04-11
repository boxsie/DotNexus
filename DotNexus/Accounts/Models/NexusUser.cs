namespace DotNexus.Account.Models
{
    public class NexusUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int Pin { get; set; }
        public GenesisId GenesisId { get; set; }

        public (string, string) GetLookupKeyVal(string genesisName = nameof(GenesisId.Genesis))
        {
            var useGenesis = !string.IsNullOrWhiteSpace(GenesisId?.Genesis);
            var key = useGenesis ? genesisName : nameof(Username).ToLower();
            var val = useGenesis ? GenesisId?.Genesis : Username;

            return (key, val);
        }
    }
}