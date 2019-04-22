using System.Threading;
using System.Threading.Tasks;

namespace DotNexus.Core.Nexus
{
    public interface INexusService
    {
        Task<T> GetAsync<T>(string path, NexusRequest request, CancellationToken token = default, bool logOutput = true);
    }
}