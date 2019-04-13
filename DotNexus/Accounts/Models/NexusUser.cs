namespace DotNexus.Accounts.Models
{
    public class NexusUser
    {
        public string Username { get; set; }
        public GenesisId GenesisId { get; set; }
        public int? Pin { get; set; }
    }

    public class NexusUserCredential
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int? Pin { get; set; }
    }
}