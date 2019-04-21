using System.Threading.Tasks;
using Boxsie.Wrapplication;

namespace DotNexus.App
{
    public class DotNexusApp : IBxApp
    {
        public DotNexusApp() { }

        public Task StartAsync()
        {
            return Task.CompletedTask;
        }
    }
}