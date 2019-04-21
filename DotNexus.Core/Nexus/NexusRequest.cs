using System.Collections.Generic;
using System.Linq;

namespace DotNexus.Core.Nexus
{
    public class NexusRequest
    {
        public Dictionary<string, string> Param { get; }

        public NexusRequest(Dictionary<string, string> param)
        {
            Param = param;
        }

        public string GetParamString()
        {
            return string.Join('&', Param.Select(x => $"{x.Key}={x.Value}"));
        }
    }
}
