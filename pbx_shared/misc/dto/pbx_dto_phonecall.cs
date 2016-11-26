using pbx_shared;
using System;
using System.Collections.Generic;

namespace pbxlib.dto
{
    public class pbx_dto_phonecall
    {
        public int callid { get; set; }
        public string remotenbr { get; set; }
        public string dialednbr { get; set; }
        public string localnbr { get; set; }
        public DateTime starttime { get; set; }
        public DateTime endtime { get; set; }
        //public enum calldirection { inbound, outbound }
        public phonecall.calldirection direction = phonecall.calldirection.inbound;

        public phonecall.callstates callstate = phonecall.callstates.active;

        public List<string> connected_extensions = new List<string>();

        public pbx_dto_phonecall(phonecall call)
        {
            this.callid = call.switchspecificcallid;
            this.localnbr = call.localnbr;
            this.remotenbr = call.remotenbr;
            this.starttime = call.starttime;
            this.dialednbr = call.dialednbr;
            this.direction = call.direction;
            this.callstate = call.callstate;

            foreach (var ext in call.connected_extensions)
            {
                this.connected_extensions.Add(ext.extension_nbr);
            }
        }

        public pbx_dto_phonecall() { }
    }
}