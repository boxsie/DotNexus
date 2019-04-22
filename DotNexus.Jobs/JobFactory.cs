using System;
using System.Threading;
using System.Threading.Tasks;
using DotNexus.Core.Ledger;
using DotNexus.Core.Ledger.Models;
using DotNexus.Core.Nexus;
using DotNexus.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DotNexus.Jobs
{
    public class JobFactory : IJobFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly INexusServiceFactory _serviceFactory;

        public JobFactory(ILoggerFactory loggerFactory, INexusServiceFactory serviceFactory)
        {
            _loggerFactory = loggerFactory;
            _serviceFactory = serviceFactory;
        }

        public async Task<BlockNotifyJob> CreateBlockNotifyJob(NexusNodeEndpoint endpoint, TimeSpan interval, Func<Block, Task> onNotify, CancellationToken token = default)
        {
            var job = new BlockNotifyJob(
                _loggerFactory.CreateLogger<BlockNotifyJob>(), 
                _serviceFactory.Get<LedgerService>(endpoint),
                onNotify);

            await job.StartAsync(interval, token);

            return job;
        }
    }
}