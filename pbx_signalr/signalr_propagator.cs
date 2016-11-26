using Microsoft.AspNet.SignalR.Hubs;
using pbx_dto_lib;
using pbx_shared.serverpush;

namespace pbx_signalr
{
    /*
     * msg_propagator 
     * 
     * wrapper class to simplify integration
     * 
     * implements the 'propagator' interface 
     * by implementing the interface, we can incorporate into the pbx's message propagator queue
     * and be handled in the same process flow as the standard propagation to a monitor system
     * rather than having two functional areas to distribute the event to the two different client groups.
     */
    public class signalr_propagator : signalr_broadcaster, ipbx_msgpropagator
    {
        public void propagatepbxevent(pbx_dto dto) { this.BroadcastPbxEvent(dto); }

        public signalr_propagator(IHubConnectionContext<dynamic> clients) : base(clients) { }

        public bool enabled { get; set; } = true;

    }

}