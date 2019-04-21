using DotNexus.Core.Nexus;
using Microsoft.AspNetCore.Http;

namespace DotNexus.Identity
{
    public interface INexusServiceFactory
    {
        T Get<T>(HttpContext context) where T : NexusService;
        T Get<T>(NexusNodeParameters nodeParams) where T : NexusService;
    }
}