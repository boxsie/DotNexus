using DotNexus.Core.Nexus;
using Microsoft.AspNetCore.Http;

namespace DotNexus.Identity
{
    public class NodeManager : INodeManager
    {
        public const string NodeIdClaimType = "NodeId";

        public NexusNodeParameters GetCurrentParameters(HttpContext context)
        {
            return new NexusNodeParameters
            {
                Url = "http://serves:8080/;",
                Username = "username",
                Password = "password",
                Settings = new NexusNodeSettings
                {
                    ApiSessions = true,
                    IndexHeight = true
                }
            };
        }

        public NexusNodeParameters GetNodeParameters(string nodeId)
        {
            throw new System.NotImplementedException();
        }
    }
}