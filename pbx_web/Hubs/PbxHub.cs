using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using pbx_lib;
using pbx_dto_lib;

namespace pbx_web
{

    /*
     * PbxHub
     * 
     * signalr hub for phone system clients
     * receives incoming commands from signalr clients and forwards to the phone system module
     */
    [HubName("pbx")]
    public class PbxHub : Hub
    {
        /* private reference to WebApiApplication.pbx (set in constructor) */
        private readonly pbx _pbx;

        /* constructors */
        public PbxHub() : this(WebApiApplication._pbx) {}
        public PbxHub(pbx switch_driver) { this._pbx = switch_driver;}

        /* public api */
        public pbx_dto_fullstatus getsystemstate() { return this._pbx.getfullstatusdto(); }
        public void answercall(int callid) { this._pbx.answercall(callid); }
        public void endcall(int callid) { this._pbx.endcall(callid); }
        public void holdcall(int callid) { this._pbx.holdcall(callid); }
        public void resumecall(int callid) { this._pbx.resumecall(callid); }
        public void connect_extension(int callid, string extension_nbr) { this._pbx.connect_extension(callid, extension_nbr); }
        public void disconnect_extension(int callid, string extension_nbr) { this._pbx.disconnect_extension(callid, extension_nbr); }

    }


}