using pbx_shared.dto;
using pbx_shared.eventreceivers;

namespace pbx_msmq_integration
{

    /* receiver_wrapper_base 
     * 
     * abstract base class for user-implementation of ipbx_eventreceiver 
     * 
     * implements pbxlib.ipbx_eventreceiver which means a single delegate
     * to handle an event received callback (ipbx_call_event_delegates.pbxevent).
     * 
     * user can derive a class from this class and override the event_callback
     */
    public abstract class receiver_wrapper_base : ipbx_eventreceiver
    {
        /* the interface implementation is a single delegate */
        public ipbx_call_event_delegates.pbxevent pbxeventreceived { get; set; }

        public string queue_name { get; set; }


        /* constructor */
        public receiver_wrapper_base(string queue_name) {
            this.pbxeventreceived += event_callback;
            this.queue_name = queue_name;
        }


        /* the overridable local top-level callback entry point */
        public abstract void event_callback(object sender, pbx_dto dto);


        /* start the receiving queue and being receiving event notifications */
        public void start() { new receiver(this.queue_name, this.pbxeventreceived).start(); }

    }
}
