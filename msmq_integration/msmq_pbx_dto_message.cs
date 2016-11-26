using pbx_shared.dto;
using pbx_shared.misc;
using System;
using System.Collections.Generic;
using System.Messaging;

namespace pbx_msmq_integration
{
    /*
     * MSMQ Phone Switch Message Types
     * 
     * custom classes derived from the standard MSMQ Message class
     * store the event type info in the label
     * store the object in the message body
     */
    [System.ComponentModel.DesignerCategory("Code")]
    public class msmq_pbx_dto_message : Message
    {
        public msmq_pbx_dto_message()
        {
            this.Priority = MessagePriority.AboveNormal;
            this.Recoverable = true;
        }

        public msmq_pbx_dto_message(pbx_dto dto) : this()
        {
            this.Body = dto;
            this.Label = dto._dto_type.ToString();
        }

        public static pbx_dto parse_msmq_message(Message msg)
        {
            // figure out what type it is from the label field
            pbx_dto.dto_type dto_type = helper.enumstring2value<pbx_dto.dto_type>(msg.Label);

            // "parse" the object back to its full type from the message body
            var dto = parsers[dto_type](msg.Body);

            return dto;
        }

        /* type-specific message-parsing routines */
        class dto_parser_dictionary : Dictionary<pbx_dto.dto_type, Func<object, pbx_dto>> { }
        static dto_parser_dictionary parsers = new dto_parser_dictionary() {
                { pbx_dto.dto_type.callanswered, (message_body) => { return (pbx_dto_callanswered)message_body; } },
                { pbx_dto.dto_type.callended, (message_body) => { return (pbx_dto_callended)message_body; } },
                { pbx_dto.dto_type.callreceived, (message_body) => { return (pbx_dto_callreceived)message_body; } },
                { pbx_dto.dto_type.callstatechanged, (message_body) => { return (pbx_dto_callstatechanged)message_body; } },
                { pbx_dto.dto_type.calltransferred, (message_body) => { return (pbx_dto_calltransferred)message_body; } },
                { pbx_dto.dto_type.secondary_extension_added, (message_body) => { return (pbx_dto_secondary_extension_added)message_body; } },
                { pbx_dto.dto_type.secondary_extension_removed, (message_body) => { return (pbx_dto_secondary_extension_removed)message_body; } },
                { pbx_dto.dto_type.unknown, (message_body) => { return (pbx_dto)message_body; } }
        };


    }

}
