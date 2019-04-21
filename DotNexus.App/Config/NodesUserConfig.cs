using System.Collections.Generic;
using Boxsie.Wrapplication.Config.Contracts;
using Boxsie.Wrapplication.Storage;
using DotNexus.Core.Nexus;

namespace DotNexus.App.Config
{
    public class NodesUserConfig : IUserConfig
    {
        public Dictionary<string, NexusNodeParameters> Nodes { get; set; }

        public void SetDefault()
        {
            Nodes = new Dictionary<string, NexusNodeParameters>();
        }
    }
}