using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DotNexus.Core.Nexus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace DotNexus.Identity
{
    public interface INodeManager
    {
        Task<NexusNodeEndpoint> GetCurrentEndpointAsync(HttpContext context);
        Task<NexusNodeEndpoint> GetEndpointAsync(string nodeId);
        Task<IEnumerable<NexusNodeEndpoint>> GetAllEndpointsAsync();
        Task CreateAsync(NexusNodeEndpoint nodeEndpoint);
        Task<IdentityResult> LoginAsync(HttpContext httpContext, NexusNodeEndpoint nodeEndpoint, bool isPersistent = false, CancellationToken token = default);
    }
}