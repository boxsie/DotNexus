using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNexus.Core.Ledger.Models;
using DotNexus.Identity;
using DotNexus.Jobs;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace DotNexus.App.Hubs
{
    public class BlockhainHubContext
    {
        private readonly IHubContext<BlockchainHub> _hubContext;
        private readonly INodeManager _nodeManager;
        private readonly BlockNotifyJob _notifyJob;

        public BlockhainHubContext(IHubContext<BlockchainHub> hubContext, IJobFactory jobFactory, INodeManager nodeManager)
        {
            _hubContext = hubContext;
            _nodeManager = nodeManager;
        }

        public Task NewBlockNotify(Block block)
        {
            return _hubContext.Clients.All.SendAsync("newBlockNotify", block);
        }
    }

    public class BlockchainHub : Hub
    {
        public string NodeId { get; set; }

        public BlockchainHub() { }

        public override Task OnConnectedAsync()
        {
            var nodeId = Context.User.FindFirst(NodeManager.NodeIdClaimType);
            return base.OnConnectedAsync();
        }
    }
}
