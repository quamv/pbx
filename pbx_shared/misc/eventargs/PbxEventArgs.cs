using pbx_shared;
using System;

namespace pbxlib.eventargs
{
    /* 
     * pbx-related eventargs 
     * 
     * captures the details of the various pbx related events

 *      */
    public class PbxEventArgs : EventArgs
    {
        public enum pbx_eventtypes { callreceived, endcall, trunkstatechange, calltransferred, callstatechange, message, extension_connected};
        public int switchid { get; set; }
        public pbx_eventtypes eventtype { get; set; }
    }

    public class PbxEventArgs_NewCall : PbxEventArgs
    {
        //public pbx _phoneswitch { get; set; }
        public phonecall call { get; set; }

        public PbxEventArgs_NewCall(int switchid, phonecall call)
        {
            this.switchid = switchid;
            this.eventtype = pbx_eventtypes.callreceived;
            this.call = call;
        }

        // required for de-serialization with xmlmessageformatter
        public PbxEventArgs_NewCall() { }
    }

    public class PbxEventArgs_CallEnded : PbxEventArgs
    {
        public int switchspecificcallid { get; set; }
        public int phonesystemcallid { get; set; }
        public DateTime endtime { get; set; }

        public PbxEventArgs_CallEnded(int switchid, int callid, DateTime endtime)
        {
            this.switchid = switchid;
            this.eventtype = pbx_eventtypes.endcall;
            this.switchspecificcallid = callid;
            this.endtime = endtime;
        }

        // required for de-serialization with xmlmessageformatter
        public PbxEventArgs_CallEnded() { }
    }

    public class PbxEventArgs_CallStateChange : PbxEventArgs
    {
        public int callid { get; set; }
        public phonecall.callstates newstate { get; set; }
        public DateTime eventtime { get; set; }

        public PbxEventArgs_CallStateChange(int callid, phonecall.callstates newstate)
        {
            this.switchid = switchid;
            this.eventtype = pbx_eventtypes.callstatechange;
            this.callid = callid;
            this.eventtime = eventtime;
            this.newstate = newstate;
        }

        // required for de-serialization with xmlmessageformatter
        public PbxEventArgs_CallStateChange() { }
    }

    public class PbxEventArgs_ExtensionConnected : PbxEventArgs
    {
        public int callid { get; set; }
        public string extension_nbr { get; set; }
        public string addremove { get; set; }
        public DateTime eventtime { get; set; }

        public PbxEventArgs_ExtensionConnected(int callid, string extension_nbr)
        {
            this.switchid = switchid;
            this.eventtype = pbx_eventtypes.extension_connected;
            this.callid = callid;
            this.eventtime = eventtime;
            this.extension_nbr = extension_nbr;
            this.addremove = "add";
        }

        // required for de-serialization with xmlmessageformatter
        public PbxEventArgs_ExtensionConnected() { }
    }


    public class PbxEventArgs_CallTransfer : PbxEventArgs
    {
        public int callid { get; set; }
        public string from_extension { get; set; }
        public string to_extension { get; set; }
        public DateTime eventtime { get; set; }

        public PbxEventArgs_CallTransfer(int callid, string from_extension, string to_extension)
        {
            this.switchid = switchid;
            this.eventtype = pbx_eventtypes.calltransferred;
            this.callid = callid;
            this.eventtime = eventtime;
            this.from_extension = from_extension;
            this.to_extension = to_extension;
        }

        // required for de-serialization with xmlmessageformatter
        public PbxEventArgs_CallTransfer() { }
    }


    public class PbxEventArgs_Message : PbxEventArgs
    {
        public string msgtext { get; set; }

        public PbxEventArgs_Message(string msg)
        {
            this.eventtype = pbx_eventtypes.message;
            this.msgtext = msg;
        }
    }

    public class PbxEventArgs_TrunkStateChange : PbxEventArgs
    {
        public string trunkid { get; set; }
        public phonenumber.trunkstates newstate { get; set; }

        // required for de-serialization with xmlmessageformatter
        public PbxEventArgs_TrunkStateChange() { }

        public PbxEventArgs_TrunkStateChange(string trunkid, phonenumber.trunkstates newstate)
        {
            this.trunkid = trunkid;
            this.newstate = newstate;
            this.eventtype = pbx_eventtypes.trunkstatechange;
        }
    }

}
