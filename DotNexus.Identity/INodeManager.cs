using DotNexus.Core.Nexus;
using Microsoft.AspNetCore.Http;

namespace DotNexus.Identity
{
    public interface INodeManager
    {
        NexusNodeParameters GetCurrentParameters(HttpContext context);
        NexusNodeParameters GetNodeParameters(string nodeId);
    }
}