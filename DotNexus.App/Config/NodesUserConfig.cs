using System.Collections.Generic;
using Boxsie.Wrapplication.Config.Contracts;
using Boxsie.Wrapplication.Storage;
using DotNexus.Core.Nexus;

namespace DotNexus.App.Config
{
    public class NodeConnection
    {
        public string Url { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public NexusSettings Settings { get; set; }
    }

    public class NodesUserConfig : IUserConfig
    {
        public Dictionary<string, NodeConnection> Nodes { get; set; }

        public void SetDefault()
        {
            Nodes = new Dictionary<string, NodeConnection>();
        }
    }
}