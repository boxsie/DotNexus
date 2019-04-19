using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNexus.Jobs;
using DotNexus.Ledger;
using DotNexus.Ledger.Models;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace DotNexus.App.Hubs
{
    public class BlockhainHubContext
    {
        private readonly IHubContext<BlockchainHub> _hubContext;
        private readonly BlockNotifyJob _notifyJob;
        private readonly Guid _notifyId;

        public BlockhainHubContext(IHubContext<BlockchainHub> hubContext, BlockNotifyJob notifyJob)
        {
            _hubContext = hubContext;
            _notifyJob = notifyJob;
            _notifyId = _notifyJob.Subscribe(NewBlockNotify);
        }

        public Task NewBlockNotify(Block block)
        {
            return _hubContext.Clients.All.SendAsync("newBlockNotify", block);
        }
    }

    public class BlockchainHub : Hub
    {
        public BlockchainHub() { }
    }
}
