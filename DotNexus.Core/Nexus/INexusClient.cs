using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DotNexus.Core.Nexus
{
    public interface INexusClient
    {
        void ConfigureHttpClient(NexusNodeParameters parameters);
        Task<HttpResponseMessage> GetAsync(string path, string logHeader, NexusRequest request, CancellationToken token, bool logOutput);
        Task<HttpResponseMessage> PostAsync(string path, string logHeader, NexusRequest request, CancellationToken token, bool logOutput);
    }
}