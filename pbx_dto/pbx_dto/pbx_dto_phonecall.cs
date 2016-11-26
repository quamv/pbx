using System;
using System.Collections.Generic;

namespace pbx_dto_lib
{
    public class pbx_dto_phonecall
    {
        public int callid { get; set; }
        public string remotenbr { get; set; }
        public string dialednbr { get; set; }
        public string localnbr { get; set; }
        public DateTime starttime { get; set; }
        public DateTime endtime { get; set; }

        public enum calldirection { inbound, outbound }
        public calldirection direction = calldirection.inbound;

        public enum callstates { ringing, hold, active, ended, unknown, ringing_again }
        public callstates callstate = callstates.active;

        public List<string> connected_extensions = new List<string>();

        public pbx_dto_phonecall(pbx_dto_phonecall call)
        {
            this.callid = call.callid;
            this.localnbr = call.localnbr;
            this.remotenbr = call.remotenbr;
            this.starttime = call.starttime;
            this.dialednbr = call.dialednbr;
            this.direction = call.direction;
            this.callstate = call.callstate;

            foreach (var ext in call.connected_extensions)
            {
                this.connected_extensions.Add(ext);
            }
        }

        public pbx_dto_phonecall() { }
    }
}