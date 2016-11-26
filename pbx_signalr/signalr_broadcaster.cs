using Microsoft.AspNet.SignalR.Hubs;
using pbx_dto_lib;
using pbx_shared.serverpush;

namespace pbx_signalr
{
    /*
     * msg_distributor 
     * 
     * sends updates to connected signalr clients. 
     */
    public class signalr_broadcaster : ipbx_directclients_msgdistributor
    {
        private IHubConnectionContext<dynamic> Clients { get; set; }

        public signalr_broadcaster(IHubConnectionContext<dynamic> clients) { this.Clients = clients; }

        public void BroadcastPbxEvent(pbx_dto dto) { Clients.All.pbxevent(dto); }
    }


}