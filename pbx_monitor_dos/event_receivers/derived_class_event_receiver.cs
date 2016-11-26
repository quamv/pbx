using pbx_msmq_integration;
using pbx_shared.dto;

namespace pbx_monitor_dos.event_receivers
{
    // a base-class/derived-class implementation of a receiver
    public class derived_class_event_receiver : receiver_wrapper_base
    {
        public derived_class_event_receiver() : base(Properties.Settings.Default.queue_name) { }

        public override void event_callback(object sender, pbx_dto dto) { pbx_event_handler_dos.event_callback(dto); }
    }

}
