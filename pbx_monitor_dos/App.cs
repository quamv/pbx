using pbx_monitor_dos.event_receivers;
using pbx_monitor_dos.exceptions;
using pbx_shared.dto;
using pbx_shared.misc;
using System;
using System.Collections.Generic;
using System.Threading;

/* pbx_monitor_dos - a dos-based implementation of a pbx event monitor */
namespace pbx_monitor_dos
{
    class App
    {

        /* program entry point */
        static void Main_BAK(string[] args)
        {
            Console.WriteLine("MSMQ Queue Name: " + Properties.Settings.Default.queue_name);

            // determine the type of event receiver to use based on the property setting value
            var rcvr_type_s = Properties.Settings.Default.receiver_type;

            Console.WriteLine("Receiver Type: " + rcvr_type_s);


            try
            {
                available_receiver_types rcvr_type;


                try
                {
                    // convert the string to the enum value, barf on invalid
                    rcvr_type = helper.enumstring2value<available_receiver_types>(rcvr_type_s);
                }
                catch (ArgumentException) { throw new invalid_receiver_type(rcvr_type_s); }


                Console.WriteLine("Starting receiver...");

                receiver_starter[rcvr_type]();

                Console.WriteLine("Receiver started... putting main thread to sleep...");

                // block/sleep until Ctrl-C (the receiver thread does not keep the app alive)
                new ManualResetEvent(false).WaitOne();

            }
            catch (invalid_receiver_type) { invalid_receiver_type(rcvr_type_s); }
            catch (Exception ex) { Console.WriteLine("Error: " + ex.Message); }
        }

        public enum available_receiver_types { simple, derived, injection, blockingqueue, blockingqueue2, derivedqueue };


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


        static Dictionary<available_receiver_types, Action> receiver_starter = new Dictionary<available_receiver_types, Action>()
        {
            { available_receiver_types.blockingqueue, () => { new blocking_q_based_receiver().start(); } },
//derivedqueue
//            { available_receiver_types.derived, () => { new derived_class_event_receiver( .start(); } },
            //{ available_receiver_types.blockingqueue2, () => { new blocking_pbx_dto_queu.start(); } },
            //{ available_receiver_types.blockingqueue2, () => { new blocking_pbx_dto_queue().start(); } },
            { available_receiver_types.derived, () => {  new derived_class_event_receiver().start();  } },
            { available_receiver_types.simple, () => { new simplest_event_receiver().start(); } },
            { available_receiver_types.injection, () => {
                    var factory = new pbx_msmq_integration.receiver_factory(Properties.Settings.Default.queue_name);
                    new injectable_event_receiver(factory).start();
                }
            }
        };

        static void invalid_receiver_type(string receiver_type)
        {
            Console.WriteLine("Invalid property setting 'receiver_type' " + receiver_type);
            Console.WriteLine("Valid values:");
            foreach (var rcvr_type in Properties.Settings.Default.receiver_types)
            {
                Console.WriteLine(rcvr_type);
            }
        }

    }
}
