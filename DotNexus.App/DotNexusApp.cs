using System.Threading.Tasks;
using Boxsie.Wrapplication;
using Boxsie.Wrapplication.Config;

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