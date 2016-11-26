using System;
using System.Collections.Generic;

namespace pbx_dto_lib
{

    ///*
    // * phonecall
    // * 
    // * a call record
    // */
    //public class phonecall
    //{
    //    public string remotenbr { get; set; }
    //    public string dialednbr { get; set; }
    //    public string localnbr { get; set; }
    //    public DateTime starttime { get; set; }
    //    public DateTime endtime { get; set; }
    //    public enum calldirection { inbound, outbound }
    //    public calldirection direction = calldirection.inbound;
    //    public int switchid { get; set; }
    //    public DateTime initiallyansweredat { get; set; }
    //    public int totalholdms { get; set; }
    //    public int switchspecificcallid { get; set; }
    //    public int phonesystemcallid { get; set; }
    //    //public List<supportrep> ActiveReps { get; set; }

    //    public enum callstates { ringing, hold, active, ended, unknown, ringing_again }
    //    public callstates callstate { get; set; }

    //    public List<extension> connected_extensions { get; set; }

    //    public phonecall()
    //    {
    //        //ActiveReps = new List<supportrep>();
    //        connected_extensions = new List<extension>();
    //        this.starttime = DateTime.Now;
    //        this.endtime = DateTime.Now;
    //        this.initiallyansweredat = DateTime.Now;

    //    }

    //    public void applyupdates(phonecall updates)
    //    {
    //        if (this.phonesystemcallid != updates.phonesystemcallid)
    //        {
    //            throw new Exception("call::applyupdates phonesystemcallid mismatch");
    //        }

    //        this.callstate = updates.callstate;
    //        this.endtime = updates.endtime;
    //        this.initiallyansweredat = updates.initiallyansweredat;
    //        this.localnbr = updates.localnbr;
    //        this.remotenbr = updates.remotenbr;
    //        this.starttime = updates.starttime;
    //    }
    //}


    public class pbx_dto
    {
        public enum dto_type
        {
            callreceived,
            callanswered,
            calltransferred,
            callstatechanged,
            callended,
            secondary_extension_added,
            secondary_extension_removed,
            unknown
        }

        public dto_type _dto_type { get; set; }

        public DateTime createdate { get; set; }

        public pbx_dto() { this.createdate = DateTime.Now; }
    }

    public class pbx_dto_callreceived : pbx_dto
    {
        public pbx_dto_callreceived() : base() { }
        public pbx_dto_callreceived(pbx_dto_phonecall call_dto) : base()
        {
            this.call_dto = call_dto;
        }

        public pbx_dto_phonecall call_dto { get; set; }

        //public pbx_dto_callreceived(phonecall call) : this(new pbx_dto_phonecall(call)) { }

    }

    public class pbx_dto_callupdate_base : pbx_dto
    {
        public int callid { get; set; }
        public pbx_dto_callupdate_base(int callid) { this.callid = callid; }
        public pbx_dto_callupdate_base() { }
    }

    public class pbx_dto_callanswered : pbx_dto_callupdate_base
    {
        public pbx_dto_callanswered(int callid) : base(callid)
        {
            this._dto_type = dto_type.callanswered;
        }
        public pbx_dto_callanswered() { }
    }

    public class pbx_dto_callstatechanged : pbx_dto_callupdate_base
    {
        public pbx_dto_phonecall.callstates newstate { get; set; }

        public pbx_dto_callstatechanged() { }
        public pbx_dto_callstatechanged(int callid) : base(callid)
        {
            this._dto_type = dto_type.callstatechanged;
        }
        public pbx_dto_callstatechanged(int callid, pbx_dto_phonecall.callstates newstate) : base(callid)
        {
            this._dto_type = dto_type.callstatechanged;
            this.newstate = newstate;
        }
    }

    public class pbx_dto_callended : pbx_dto_callupdate_base
    {
        public pbx_dto_callended() { }
        public pbx_dto_callended(int callid) : base(callid)
        {
            this._dto_type = dto_type.callended;
        }
    }

    public class pbx_dto_callandextension : pbx_dto_callupdate_base
    {
        public string extension_nbr { get; set; }

        public pbx_dto_callandextension() { }
        public pbx_dto_callandextension(int callid, string extension_nbr) : base(callid) { this.extension_nbr = extension_nbr; }
    }

    public class pbx_dto_secondary_extension_added : pbx_dto_callandextension
    {
        public pbx_dto_secondary_extension_added() { }
        public pbx_dto_secondary_extension_added(int callid, string extension_nbr) : base(callid, extension_nbr)
        {
            this._dto_type = dto_type.secondary_extension_added;
        }
    }

    public class pbx_dto_secondary_extension_removed : pbx_dto_callandextension
    {
        public pbx_dto_secondary_extension_removed() {  }
        public pbx_dto_secondary_extension_removed(int callid, string extension_nbr) : base(callid, extension_nbr)
        {
            this._dto_type = dto_type.secondary_extension_removed;
        }
    }

    public class pbx_dto_calltransferred : pbx_dto
    {
        public int callid { get; set; }
        public string from_extension { get; set; }
        public string to_extension { get; set; }
        public pbx_dto_calltransferred(int callid, string from_extension, string to_extension) 
        {
            this._dto_type = dto_type.calltransferred;
            this.from_extension = from_extension;
            this.to_extension = to_extension;
        }
        public pbx_dto_calltransferred() { }

    }

    /*
     * phonenumber
     * 
     * a phonenumber line coming into a location
     */
    public class pbx_dto_phonenumber
    {
        public readonly object linelock = new object();
        public enum trunktypes { analog, t1 }
        public enum trunkstates { idle, ringing, connected, cleanup, unknown, offline, hold, busy }
        public trunktypes linetype { get; set; }
        public trunkstates linestate { get; set; }
        public string phonenbr { get; set; }

        ////public void applyupdate(phonenumber update)
        ////{
        ////    this.linestate = update.linestate;
        ////}

        //public pbx_dto_phonenumber()
        //{
        //    this.caller_id = "";
        //    this.active = true;
        //    this.caller_id_mode = caller_id_modes.none;
        //    this.caller_id_prefix = "";
        //    this.show_callerid_on_outbound = true;
        //}

        //public bool active { get; set; }
        //public string caller_id { get; set; }
        //public bool show_callerid_on_outbound { get; set; }
        //public enum caller_id_modes { none, your_text, your_text_plus_original }
        //public caller_id_modes caller_id_mode { get; set; }
        //public string caller_id_prefix { get; set; }

        //public call_route_destination default_route { get; set; }
        //public call_route_destination holiday_route { get; set; }

        //public hold_music_group hold_music_group { get; set; }
        //public bool screen_for_privacy { get; set; }

    }

    public class pbx_dto_fullstatus
    {
        public List<pbx_dto_phonenumber> phonenumbers;
        public List<pbx_dto_phonecall> calls;
        public DateTime timestamp;

        public pbx_dto_fullstatus()
        {

        }

    }

}
