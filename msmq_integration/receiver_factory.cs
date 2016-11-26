using pbx_shared.eventreceivers;

namespace pbx_msmq_integration
{
    /*
     * receiver_factory
     * 
     * creates/returns/produces an ipbx_eventreceiver
     * this class gets injected into phonesystemmonitor at construction
     * allows phonesystemmonitor to get an event receiver without knowing the details
     * currently it is an MSMQ based queue but could be replaced by whatever and you
     * would create a new similar class and pass that into phonesystemmonitor constructor
     * 
     * injections (into this class):
     * the callback delegate for pbx events
     */
    public class receiver_factory : ipbx_eventreceiver_factory
    {
        public string queue_name { get; set; }

        /* constructor */
        public receiver_factory(string queue_name) { this.queue_name = queue_name; }

        
        /* factory function */
        public ipbx_eventreceiver getpbxeventreceiver(ipbx_call_event_delegates.pbxevent pbxevent)
        {
            return new receiver(this.queue_name, pbxevent);
        }
    }

}