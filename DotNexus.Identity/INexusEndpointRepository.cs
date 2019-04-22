using System.Collections.Generic;
using System.Threading.Tasks;
using DotNexus.Core.Nexus;

namespace DotNexus.Identity
{
    public interface INexusEndpointRepository
    {
        Task CreateNodeAsync(NexusNodeEndpoint endpoint);
        Task<IEnumerable<NexusNodeEndpoint>> GetNodesAsync();
        Task<NexusNodeEndpoint> GetNodeAsync(string nodeId);
    }
}