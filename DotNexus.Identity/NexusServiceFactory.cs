using System;
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

        public T Get<T>(HttpContext context) where T : NexusService
        {
            var nexusNode = new NexusNode(_serviceProvider.GetService<INexusClient>(), _nodeManager.GetCurrentParameters(context));

            var service = Activator.CreateInstance(typeof(T), nexusNode, _serviceProvider.GetService<ILogger>());

            return (T)service;
        }

        public T Get<T>(NexusNodeParameters nodeParams) where T : NexusService
        {
            var nexusNode = new NexusNode(_serviceProvider.GetService<INexusClient>(), nodeParams);

            var service = Activator.CreateInstance(typeof(T), nexusNode, _serviceProvider.GetService<ILogger>());

            return (T)service;
        }
    }
}