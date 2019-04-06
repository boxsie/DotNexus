namespace DotNexus.Ledger.Models
{
    public class Tx
    {
        public string Type { get; set; }
        public int Version { get; set; }
        public int Sequence { get; set; }
        public int Timestamp { get; set; }
        public string Genesis { get; set; }
        public string NextHash { get; set; }
        public string PrevHash { get; set; }
        public string PubKey { get; set; }
        public string Signature { get; set; }
        public string Hash { get; set; }
        public string Operation { get; set; }
    }
}