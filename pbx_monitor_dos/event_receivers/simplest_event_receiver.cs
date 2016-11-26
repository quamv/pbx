using pbx_msmq_integration;
using pbx_shared.dto;

namespace pbx_monitor_dos.event_receivers
{
    // no class hierarchy or injection required
    public class simplest_event_receiver
    {
        public void event_callback(object sender, pbx_dto dto) { pbx_event_handler_dos.event_callback(dto); }

        // normally we'd use 'containment' via member prop. for our purposes, we don't even need containment, just create and start.
        public void start() { new receiver(Properties.Settings.Default.queue_name, this.event_callback).start(); }
    }
}
