using System.Collections.Generic;
using DotNexus.Core.Ledger.Models;

namespace DotNexus.App.Models
{
    public class GenesisViewModel
    {
        public string Genesis { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}