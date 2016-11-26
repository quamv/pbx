using msmq_base_support;
using pbx_shared.dto;
using pbx_shared.misc;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Messaging;

namespace pbx_msmq_integration
{
    public class blocking_pbx_dto_queue : BlockingCollection<pbx_dto>
    {

        /* a blocking queue of msmq messages */
        private blocking_msmq_queue msmq_buffer;


        /* constructor */
        public blocking_pbx_dto_queue(string name)
        {
            // default this to XmlMessageFormatter, because it works. perhaps could use others but definitely need the Type [].
            var supportedtypes = new Type[] {
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

            this.msmq_buffer = new blocking_msmq_queue(name, new XmlMessageFormatter(supportedtypes));
        }


        /* start the underlying queue and our processing thread */
        public void start() {
            this.msmq_buffer.start();
            new System.Threading.Thread(message_processing_loop).Start();
        }

        /* loop to processing incoming messages from our feeder queue */
        public void message_processing_loop()
        {
            while (true)
            {
                // take messages from our feeder queue
                var newmsg = this.msmq_buffer.Take();

                // figure out what type it is from the label field
                pbx_dto.dto_type dto_type = helper.enumstring2value<pbx_dto.dto_type>(newmsg.Label);

                // "parse" the object back to its full type from the message body
                var dto = parsers[dto_type](newmsg.Body);

                // add to meself
                this.Add(dto);
            }
        }


        /* type-specific parsing routines */
        dto_parser_dictionary parsers = new dto_parser_dictionary() {
                { pbx_dto.dto_type.callanswered, (message_body) => { return (pbx_dto_callanswered)message_body; } },
                { pbx_dto.dto_type.callended, (message_body) => { return (pbx_dto_callended)message_body; } },
                { pbx_dto.dto_type.callreceived, (message_body) => { return (pbx_dto_callreceived)message_body; } },
                { pbx_dto.dto_type.callstatechanged, (message_body) => { return (pbx_dto_callstatechanged)message_body; } },
                { pbx_dto.dto_type.calltransferred, (message_body) => { return (pbx_dto_calltransferred)message_body; } },
                { pbx_dto.dto_type.secondary_extension_added, (message_body) => { return (pbx_dto_secondary_extension_added)message_body; } },
                { pbx_dto.dto_type.secondary_extension_removed, (message_body) => { return (pbx_dto_secondary_extension_removed)message_body; } },
                { pbx_dto.dto_type.unknown, (message_body) => { return (pbx_dto)message_body; } }
        };



        /* custom dictionary just to ease the syntax, bend the brain */
        class dto_parser_dictionary : Dictionary<pbx_dto.dto_type, Func<object, pbx_dto>> { }

    }
}
