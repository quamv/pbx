using System;
using System.Collections.Generic;

namespace pbx_shared
{
    public class testclass1
    {
        public string name { get; set; }
        public testclass1() { name = "default"; extension_type = extension_types.digital; }
        //public testclass1(string name) { this.name = name; }


        public string extension_nbr { get; set; }

        public string extension_name { get; set; }

        public string outbound_callerid_phonenbr { get; set; }

        public string e911location { get; set; }

        //public call_routing_options routing_options { get; set; }

        public enum extension_types { digital, ip };

        public extension_types extension_type { get; set; }

        //public extension() { }
    }


    /*
     * phonecall
     * 
     * a call record
     */
    public class phonecall
    {
        public string remotenbr { get; set; }
        public string dialednbr { get; set; }
        public string localnbr { get; set; }
        public DateTime starttime { get; set; }
        public DateTime endtime { get; set; }
        public enum calldirection { inbound, outbound }
        public calldirection direction = calldirection.inbound;
        public int switchid { get; set; }
        public DateTime initiallyansweredat { get; set; }
        public int totalholdms { get; set; }
        public int switchspecificcallid { get; set; }
        public int phonesystemcallid { get; set; }
        //public List<supportrep> ActiveReps { get; set; }

        public enum callstates { ringing, hold, active, ended, unknown, ringing_again }
        public callstates callstate { get; set; }

        public List<extension> connected_extensions { get; set; }

        public phonecall()
        {
            //ActiveReps = new List<supportrep>();
            connected_extensions = new List<extension>();
            this.starttime = DateTime.Now;
            this.endtime = DateTime.Now;
            this.initiallyansweredat = DateTime.Now;

        }

        public void applyupdates(phonecall updates)
        {
            if (this.phonesystemcallid != updates.phonesystemcallid)
            {
                throw new Exception("call::applyupdates phonesystemcallid mismatch");
            }

            this.callstate = updates.callstate;
            this.endtime = updates.endtime;
            this.initiallyansweredat = updates.initiallyansweredat;
            this.localnbr = updates.localnbr;
            this.remotenbr = updates.remotenbr;
            this.starttime = updates.starttime;
        }
    }

}