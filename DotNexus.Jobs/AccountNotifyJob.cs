using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotNexus.Core.Ledger;
using DotNexus.Core.Ledger.Models;
using Microsoft.Extensions.Logging;

namespace DotNexus.Jobs
{
    public class AccountNotifyJob : HostedService
    {
        private readonly LedgerService _ledgerService;
        private readonly Dictionary<Guid, Func<Block, Task>> _subscriptions;

        public AccountNotifyJob(ILogger<BlockNotifyJob> logger, LedgerService ledgerService, Func<Block, Task> onNotify = null) : base(logger)
        {

        }

        protected override Task ExecuteAsync()
        {
            throw new NotImplementedException();
        }
    }
}