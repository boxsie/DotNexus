using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using DotNexus.Core.Accounts;
using DotNexus.Core.Accounts.Models;
using DotNexus.Core.Ledger;
using DotNexus.Core.Nexus;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DotNexus.Identity
{
    public class NodeManager : INodeManager
    {
        public static string NodeIdClaimType { get; private set; }

        private readonly INexusEndpointRepository _repository;
        private readonly CookieConstants _cookeConstants;

        public NodeManager(INexusEndpointRepository repository, CookieConstants cookeConstants)
        {
            _repository = repository;
            _cookeConstants = cookeConstants;

            NodeIdClaimType = _cookeConstants.NodeIdClaimType;
        }

        public Task<NexusNodeEndpoint> GetCurrentEndpointAsync(HttpContext httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
                return null;

            var nodeIdClaim = httpContext.User.FindFirst(_cookeConstants.NodeIdClaimType);

            return _repository.GetNodeAsync(nodeIdClaim.Value);
        }

        public Task<NexusNodeEndpoint> GetEndpointAsync(string nodeId)
        {
            return _repository.GetNodeAsync(nodeId);
        }

        public Task<IEnumerable<NexusNodeEndpoint>> GetAllEndpointsAsync()
        {
            return _repository.GetNodesAsync();
        }

        public async Task CreateAsync(NexusNodeEndpoint nodeEndpoint)
        {
            if (nodeEndpoint == null)
                throw new ArgumentException("Node endpoint data is missing");

            await _repository.CreateNodeAsync(nodeEndpoint);
        }

        public async Task<IdentityResult> LoginAsync(HttpContext httpContext, NexusNodeEndpoint nodeEndpoint, bool isPersistent = false, CancellationToken token = default)
        {
            if (httpContext.User.Identity.IsAuthenticated)
                return IdentityResult.Failed(new IdentityError { Description = "Already signed in" });

            var nexusNode = new NexusNode((INexusClient)httpContext.RequestServices.GetService(typeof(INexusClient)), nodeEndpoint);
            var response = await nexusNode.Client.GetAsync("ledger/mininginfo", "Connection attempt", null, token, true);

            if (!response.IsSuccessStatusCode)
                return IdentityResult.Failed(new IdentityError { Description = "Unable to connect to node endpoint" });

            var claims = new List<Claim> {new Claim(_cookeConstants.NodeIdClaimType, nodeEndpoint.Name)};
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return IdentityResult.Success;
        }

        public async Task LogoutAsync(HttpContext httpContext)
        {
            await httpContext.SignOutAsync();
        }
    }
}