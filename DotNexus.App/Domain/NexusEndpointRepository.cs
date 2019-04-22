using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Boxsie.Wrapplication.Repository;
using DotNexus.Core.Nexus;
using DotNexus.Identity;

namespace DotNexus.App.Domain
{
    public class NexusEndpointRepository : INexusEndpointRepository
    {
        private readonly IRepository<NexusNodeEndpoint> _repository;

        public NexusEndpointRepository(IRepository<NexusNodeEndpoint> repository)
        {
            _repository = repository;
        }

        public Task CreateNodeAsync(NexusNodeEndpoint endpoint)
        {
            return _repository.CreateAsync(endpoint);
        }

        public async Task<IEnumerable<NexusNodeEndpoint>> GetNodesAsync()
        {
            var nodes = await _repository.GetLastEntriesAsync();

            return nodes ?? new List<NexusNodeEndpoint>();
        }

        public async Task<NexusNodeEndpoint> GetNodeAsync(string nodeId)
        {
            var nodes = await _repository.GetWhereAsync(new List<WhereClause> { new WhereClause("Name", "=", nodeId) });

            return nodes.FirstOrDefault();
        }
    }
}
