using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DotNexus.Core.Ledger.Models;
using DotNexus.Identity;
using DotNexus.Jobs;
using Microsoft.AspNetCore.SignalR;

namespace DotNexus.App.Hubs
{
    public class BlockhainHubMessenger
    {
        private readonly IHubContext<BlockchainHub> _hubContext;
        private readonly IJobFactory _jobFactory;
        private readonly INodeManager _nodeManager;
        private readonly Dictionary<string, (BlockNotifyJob, CancellationTokenSource)> _notifyJobs;

        public BlockhainHubMessenger(IHubContext<BlockchainHub> hubContext, IJobFactory jobFactory, INodeManager nodeManager)
        {
            _hubContext = hubContext;
            _jobFactory = jobFactory;
            _nodeManager = nodeManager;
            _notifyJobs = new Dictionary<string, (BlockNotifyJob, CancellationTokenSource)>();
        }

        public async Task StartBlockNotify(string nodeId)
        {
            if (_notifyJobs.ContainsKey(nodeId))
            {
                var job = _notifyJobs[nodeId];
                await job.Item1.StopAsync(job.Item2.Token);
                await job.Item1.StartAsync(job.Item2.Token);
            }
            else
            {
                var node = await _nodeManager.GetEndpointAsync(nodeId);
                var ts = new CancellationTokenSource();
                var job = await _jobFactory.CreateBlockNotifyJobAsync(node, TimeSpan.FromSeconds(5), NewBlockNotify, ts.Token);

                _notifyJobs.Add(nodeId, (job, ts));
            }
        }

        public Task NewBlockNotify(Block block)
        {
            return _hubContext.Clients.All.SendAsync("newBlockNotify", block);
        }
    }
}