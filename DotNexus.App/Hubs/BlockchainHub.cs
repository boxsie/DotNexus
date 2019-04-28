using System.Linq;
using System.Threading.Tasks;
using DotNexus.Identity;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace DotNexus.App.Hubs
{
    public class BlockchainHub : Hub
    {
        private readonly BlockhainHubMessenger _messenger;

        public BlockchainHub(BlockhainHubMessenger messenger)
        {
            _messenger = messenger;
        }

        public override async Task OnConnectedAsync()
        {
            var nodeId = Context.User.FindFirst(NodeManager.NodeIdClaimType);

            await _messenger.StartBlockNotify(nodeId.Value);

            await base.OnConnectedAsync();
        }
    }
}
