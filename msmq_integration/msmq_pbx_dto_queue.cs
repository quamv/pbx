using pbx_dto_lib;
using msmq_base_support;
using System;

namespace pbx_msmq_integration
{

    /* a blocking queue interface to a msmq queue of pbx_dto objects */
    public class msmq_pbx_dto_queue : blocking_msmq_queue_of_t<pbx_dto>
    {
        /* all the various pbx_dtos (and accompanying msmq messages) */
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

        public msmq_pbx_dto_queue(string queuename)
            : base(queuename, supportedtypes, msmq_pbx_dto_message.parse_msmq_message) { }
            //: base(queuename, supportedtypes, msmq_pbx_dto_parser.parse_msmq_message) { }
    }

}
