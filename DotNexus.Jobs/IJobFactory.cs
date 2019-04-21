using System;
using System.Threading;
using System.Threading.Tasks;
using DotNexus.Core.Ledger.Models;
using DotNexus.Core.Nexus;
using Microsoft.AspNetCore.Http;

namespace DotNexus.Jobs
{
    public interface IJobFactory
    {
        Task<BlockNotifyJob> CreateBlockNotifyJob(NexusNodeParameters nodeParams, TimeSpan interval, Func<Block, Task> onNotify, CancellationToken token = default);
    }
}