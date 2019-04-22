using System;
using System.Threading.Tasks;
using DotNexus.Core.Nexus;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DotNexus.Identity
{
    public class NexusServiceFactory : INexusServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly INodeManager _nodeManager;

        public NexusServiceFactory(IServiceProvider serviceProvider, INodeManager nodeManager)
        {
            _serviceProvider = serviceProvider;
            _nodeManager = nodeManager;
        }

        public async Task<T> GetAsync<T>(HttpContext context) where T : NexusService
        {
            var nexusNode = new NexusNode(_serviceProvider.GetService<INexusClient>(), await _nodeManager.GetCurrentEndpointAsync(context));

            var service = Activator.CreateInstance(typeof(T), nexusNode, _serviceProvider.GetService<ILogger<NexusService>>());

            return (T)service;
        }

        public T Get<T>(NexusNodeEndpoint nodeEndpoint) where T : NexusService
        {
            var nexusNode = new NexusNode(_serviceProvider.GetService<INexusClient>(), nodeEndpoint);

            var service = Activator.CreateInstance(typeof(T), nexusNode, _serviceProvider.GetService<ILogger<NexusService>>());

            return (T)service;
        }
    }
}