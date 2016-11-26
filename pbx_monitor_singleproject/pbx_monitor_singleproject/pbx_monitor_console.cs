using msmq_base_support;
using pbx_dto_lib;
using System;
using System.Collections.Generic;

namespace pbx_monitor_singleproject
{
    class pbx_monitor_console
    {
        static void Main(string[] args)
        {
            var queue_name = "phoneswitchevents";// Properties.Settings.Default.queue_name;

            Console.WriteLine("MSMQ Queue Name: " + queue_name);

            iBlockingReadonlyQueue<pbx_dto> q = new blocking_msmq_queue_of_t<pbx_dto>(
                            queue_name,
                            CustomMessageClass._supportedtypes,
                            CustomMessageClass._parse_msmq_message);


            while (true)
            {
                var dto = q.Take();

                Console.WriteLine(dto._dto_type.ToString() + " Event:");

                event_handlers[dto._dto_type](dto);
            }
        }


        /*
         * event-specific handlers 
         */
        private class event_handler_dict : Dictionary<pbx_dto.dto_type, Action<pbx_dto>> { }
        private static event_handler_dict event_handlers = new event_handler_dict()
        {
            {
                pbx_dto.dto_type.callanswered,
                (dto) => { Console.WriteLine("Call ID: " + ((pbx_dto_callanswered)dto).callid); }
            },
            {
                pbx_dto.dto_type.callended,
                (dto) => {
                    var dto2 = (pbx_dto_callended)dto;
                    Console.WriteLine("Call ID: " + dto2.callid);
                    Console.WriteLine("Event Date: " + dto2.createdate);
                }
            },
            {
                pbx_dto.dto_type.callreceived,
                (dto) => {
                    var dto2 = (pbx_dto_callreceived)dto;
                    Console.WriteLine("Call Details:");
                    Console.WriteLine("Event Date: " + dto2.createdate);
                    Console.WriteLine("Direction: " + dto2.call_dto.direction);
                    Console.WriteLine("Local Number: " + dto2.call_dto.localnbr);
                    Console.WriteLine("Dialed #: " + dto2.call_dto.dialednbr);
                }
            },
            {
                pbx_dto.dto_type.callstatechanged,
                (dto) => {
                    var dto2 = (pbx_dto_callstatechanged)dto;
                    Console.WriteLine("Call ID: " + dto2.callid);
                    Console.WriteLine("Event Date: " + dto2.createdate);
                    Console.WriteLine("New State: " + dto2.newstate);
                }
            },
            {
                pbx_dto.dto_type.calltransferred,
                (dto) => {
                    var dto2 = (pbx_dto_calltransferred)dto;
                    Console.WriteLine("Call ID: " + dto2.callid);
                    Console.WriteLine("Event Date: " + dto2.createdate);
                    Console.WriteLine("From Ext #: " + dto2.from_extension);
                    Console.WriteLine("To Ext #: " + dto2.to_extension);
                }
            },
            {
                pbx_dto.dto_type.secondary_extension_added,
                (dto) => {
                    var dto2 = (pbx_dto_secondary_extension_added)dto;
                    Console.WriteLine("Call ID: " + dto2.callid);
                    Console.WriteLine("Event Date: " + dto2.createdate);
                    Console.WriteLine("Ext #: " + dto2.extension_nbr);
                }
            },
            {
                pbx_dto.dto_type.secondary_extension_removed,
                (dto) => {
                    var dto2 = (pbx_dto_secondary_extension_removed)dto;
                    Console.WriteLine("Call ID: " + dto2.callid);
                    Console.WriteLine("Event Date: " + dto2.createdate);
                    Console.WriteLine("Ext #: " + dto2.extension_nbr);
                }
            },
            {
                pbx_dto.dto_type.unknown,
                (dto) => {
                    Console.WriteLine("Unknown event");
                    Console.WriteLine("Event Date: " + dto.createdate);
                }
            }

        };

    }
}
