using pbx_shared.dto;


namespace pbx_shared.eventreceivers
{
    /*
     * ipbx_eventreceiver
     * 
     * interface to receive events from the pbx. a client of this interface will get
     * async notifications of incoming events via the delegates.
     */
    public interface ipbx_eventreceiver
    {
        ipbx_call_event_delegates.pbxevent pbxeventreceived { get; set; }
        void start();
    }

    /*
     * ipbx_call_event_delegates
     * 
     * just a place to define these delegates
     */
    public class ipbx_call_event_delegates
    {
        public delegate void pbxevent(object sender, pbx_dto dto);
    }



}
