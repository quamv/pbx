using pbx_msmq_integration;
using pbx_shared.dto;
using pbx_shared.eventreceivers;

namespace pbx_monitor_dos.event_receivers
{
    // no class hierarchy required. injectable factory for core receiver creation.
    public class injectable_event_receiver
    {

        /* constructor */
        public injectable_event_receiver(receiver_factory msmq_receiver_factory)
        {
            this.msmq_receiver = msmq_receiver_factory.getpbxeventreceiver(this.event_callback);
        }

        // the receiver that will monitor the msmq queue and bubble events to us
        ipbx_eventreceiver msmq_receiver { get; set; }

        // the receiver will call our callback here
        public void event_callback(object sender, pbx_dto dto) {
            pbx_event_handler_dos.event_callback(dto);
        }

        public void start() { this.msmq_receiver.start(); }
    }
}
