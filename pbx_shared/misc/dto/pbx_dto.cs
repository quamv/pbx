using pbx_shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace pbxlib.dto
{
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

        public pbx_dto_callreceived(phonecall call) : this(new pbx_dto_phonecall(call)) { }

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
        public phonecall.callstates newstate { get; set; }

        public pbx_dto_callstatechanged() { }
        public pbx_dto_callstatechanged(int callid) : base(callid)
        {
            this._dto_type = dto_type.callstatechanged;
        }
        public pbx_dto_callstatechanged(int callid, phonecall.callstates newstate) : base(callid)
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

    public class pbx_dto_fullstatus
    {
        public List<phonenumber> phonenumbers;
        public List<pbx_dto_phonecall> calls;
        public DateTime timestamp;

        public pbx_dto_fullstatus(pbx switch_driver)
        {
            this.phonenumbers = switch_driver.getallphonenumbers().ToList(); // trunks;
            this.calls = new List<pbx_dto_phonecall>();
            foreach (var nextcall in switch_driver.getallcalls().ToList())
            {
                this.calls.Add(new pbx_dto_phonecall(nextcall));
            }
            this.timestamp = DateTime.Now;
        }

        public pbx_dto_fullstatus() { }

    }

}
