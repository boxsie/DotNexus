namespace DotNexus.Account.Models
{
    public class NexusUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int Pin { get; set; }
        public GenesisId GenesisId { get; set; }
    }
}