using System.Collections.Generic;
using DotNexus.Ledger.Models;

namespace DotNexus.App.Models
{
    public class GenesisViewModel
    {
        public string Genesis { get; set; }
        public List<Tx> Transactions { get; set; }
    }
}