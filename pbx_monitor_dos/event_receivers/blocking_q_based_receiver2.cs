using pbx_msmq_integration;
using pbx_shared.dto;
using System;
using System.Threading;

namespace pbx_monitor_dos.event_receivers
{
    class blocking_pbx_dto_queueXX
    {
        // default this to XmlMessageFormatter, because it works. perhaps could use others but definitely need the Type [].
        static Type[] supportedtypes = new Type[] {
            typeof(pbx_dto),
            typeof(pbx_dto_callandextension),
            typeof(pbx_dto_callanswered),
            typeof(pbx_dto_callended),
            typeof(pbx_dto_callreceived),
            typeof(pbx_dto_callstatechanged),
            typeof(pbx_dto_calltransferred),
            typeof(pbx_dto_callupdate_base),
            //typeof(pbx_dto_fullstatus), // was causing problems. need to re-check.
            typeof(pbx_dto_phonecall),
            typeof(pbx_dto_secondary_extension_added),
            typeof(pbx_dto_secondary_extension_removed)
        };


        private void queue_processing_loop()
        {
            var q = new blocking_msmq_queue<pbx_dto>(
                Properties.Settings.Default.queue_name,
                supportedtypes,
                msmq_pbx_dto_parser.parse_msmq_message);

            q.start();

            while (true)
            {
                var dto = q.Take();

                pbx_event_handler_dos.event_callback(dto);
            }

        }

        public void start() { new Thread(queue_processing_loop).Start(); }
    }
}
