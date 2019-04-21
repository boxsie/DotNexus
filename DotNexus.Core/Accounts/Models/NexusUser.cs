namespace DotNexus.Core.Accounts.Models
{
    public class NexusUser
    {
        public string Username { get; set; }
        public GenesisId GenesisId { get; set; }
        public int? Pin { get; set; }
    }
}