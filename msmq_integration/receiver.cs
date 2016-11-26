using msmq_base_support;
using pbx_shared.dto;
using pbx_shared.eventreceivers;
using System;
using System.Collections.Generic;
using System.Messaging;

namespace pbx_msmq_integration
{
    /*
     * receiver
     * 
     * receives messages from the pbx via the member property
     * private msmq_queue_receiver thequeue
     * 
     * 'thequeue' monitors an msmq queue for incoming events and calls 
     * this.pbx_event_received which receives an MSMQ message as a paramter.
     * 
     * injections into this class (eventreceiver):
     *      ipbx_call_event_delegates.pbxevent (callback to call on event received)
     * 
     * injections into thequeue (by this class):
     *      the local callback on msmq event recieved: this.pbx_call_event_received
     *      
     * the sequence in processing an incoming message is:
     *      thequeue captures the raw msmq message and passes to us via
     *          pbx_call_event_received
     *      we parse the raw message into a typed dto and call our client callback
     *          pbxeventreceived
     */
    public class receiver : ipbx_eventreceiver
    {
        /* the actual queue */
        private msmq_queue_receiver thequeue { get; set; }

        /* delegate to call on event receipt */
        public ipbx_call_event_delegates.pbxevent pbxeventreceived { get; set; }

        
        
        /* constructor */
        public receiver(string name, ipbx_call_event_delegates.pbxevent pbx_event)
        {
            // we aint playin
            if (pbx_event == null) { throw new Exception("pbx_msmq_eventreceiver constructor callbacks must not be null"); }

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

            
            // register our own callbacks to client. we will get the raw message from the
            // base class event handler, then parse the message and raise this event
            this.pbxeventreceived += pbx_event;


            // the base msmq queue receiver
            this.thequeue = new msmq_queue_receiver(
                name,
                this.pbx_call_event_received,
                new XmlMessageFormatter(supportedtypes));

        }



        /* callback for this.thequeue when a new message is received. */
        void pbx_call_event_received(object sender, Message m) {
            var dto = parse_pbx_dto_msmq_message(m);
            this.pbxeventreceived(sender, dto);
        }

        

        /* parse an msmq message into a pbx_dto-derived object and return object */
        private pbx_dto parse_pbx_dto_msmq_message(Message m)
        {
            // convert string (e.g. 'callreceived') in the message label to enum value
            pbx_dto.dto_type dto_type = (pbx_dto.dto_type)Enum.Parse(typeof(pbx_dto.dto_type), m.Label);

            // use the parser for this type (pbx_dto_parser[dto_type]) and pass message body (m.Body)
            //return pbx_dto_parser[dto_type](m.Body);
            return parsers[dto_type](m.Body);
        }



        /* event-specific parsing routines */
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



        /* starts the base msmq reader */
        public void start() { this.thequeue.start(); }
    }


}