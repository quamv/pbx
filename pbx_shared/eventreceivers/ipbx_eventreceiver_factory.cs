
namespace pbx_shared.eventreceivers
{
    /*
     * ipbx_eventreceiver_factory
     * 
     * creates/returns/produces an ipbx_eventreceiver
     * this class gets injected into pbx at construction
     * allows pbx to get an event receiver without knowing the details
     */
    public interface ipbx_eventreceiver_factory
    {
        ipbx_eventreceiver getpbxeventreceiver(ipbx_call_event_delegates.pbxevent pbxeventreceived);
    }

}