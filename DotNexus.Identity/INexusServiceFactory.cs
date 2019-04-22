using System.Threading.Tasks;
using DotNexus.Core.Nexus;
using Microsoft.AspNetCore.Http;

namespace DotNexus.Identity
{
    public interface INexusServiceFactory
    {
        Task<T> GetAsync<T>(HttpContext context) where T : NexusService;
        T Get<T>(NexusNodeEndpoint nodeEndpoint) where T : NexusService;
    }
}