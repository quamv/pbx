using common_lib.misc;
using pbx_dto_lib;
using System;
using System.Collections.Generic;
using System.Messaging;

namespace pbx_monitor_dos_wo_integ_lib
{
    class CustomMessageClass 
    {
        /* all the various pbx_dtos (and accompanying msmq messages) */
        public static Type[] _supportedtypes = new Type[] {
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

        /* type-specific message-parsing routines */
        public class dto_parser_dictionary : Dictionary<pbx_dto.dto_type, Func<object, pbx_dto>> { }
        public static dto_parser_dictionary parsers = new dto_parser_dictionary() {
                { pbx_dto.dto_type.callanswered, (message_body) => { return (pbx_dto_callanswered)message_body; } },
                { pbx_dto.dto_type.callended, (message_body) => { return (pbx_dto_callended)message_body; } },
                { pbx_dto.dto_type.callreceived, (message_body) => { return (pbx_dto_callreceived)message_body; } },
                { pbx_dto.dto_type.callstatechanged, (message_body) => { return (pbx_dto_callstatechanged)message_body; } },
                { pbx_dto.dto_type.calltransferred, (message_body) => { return (pbx_dto_calltransferred)message_body; } },
                { pbx_dto.dto_type.secondary_extension_added, (message_body) => { return (pbx_dto_secondary_extension_added)message_body; } },
                { pbx_dto.dto_type.secondary_extension_removed, (message_body) => { return (pbx_dto_secondary_extension_removed)message_body; } },
                { pbx_dto.dto_type.unknown, (message_body) => { return (pbx_dto)message_body; } }
        };


        public static pbx_dto _parse_msmq_message(Message msg)
        {
            // figure out what type it is from the label field
            pbx_dto.dto_type dto_type = helper.enumstring2value<pbx_dto.dto_type>(msg.Label);

            // "parse" the object back to its full type from the message body
            var dto = parsers[dto_type](msg.Body);

            return dto;
        }
    }
}
