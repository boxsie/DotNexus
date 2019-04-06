using System.Collections.Generic;
using System.Linq;

namespace DotNexus.Core
{
    public class NexusRequest
    {
        private readonly Dictionary<string, string> _param;

        public NexusRequest(Dictionary<string, string> param)
        {
            _param = param;
        }

        public string GetParamString()
        {
            return string.Join('&', _param.Select(x => $"{x.Key}={x.Value}"));
        }
    }
}
