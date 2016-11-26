using pbx_shared;
using pbxlib.eventargs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace pbxlib
{

    /*
     * pbx
     * 
     * the physical hardware where the lines are plugged in
     * raises events based on line/call changes
     * 
     * injections:
     *  iphoneswitch_msgpropagator - responsible for fowarding events
     *  trunks - a list of _trunk lines the switch is supporting
     */
    [System.ComponentModel.DesignerCategory("Code")]
    public class pbx2
    {
        // the defined phonenumber lines on this switch
        private readonly ConcurrentDictionary<string, phonenumber> phonenumbers = new ConcurrentDictionary<string, phonenumber>();

        // the known extensions
        private ConcurrentDictionary<string, pbx_shared.extension> _extensions; // = new ConcurrentDictionary<string, internal_extension>();

        // an internally maintained list of 'calls'... maybe should remove and just use pure line state
        public List<phonecall> activecalls = new List<phonecall>();

        // each phone switch has unique switch id 
        public int switchid = 0;

        // each call has unique id
        private int nextcallid = 0;

        // generic object to lock on for thread/client sync
        private Object lockobject = new Object();

        // store and forward queue that uses the injected propagator
        pbx_eventpropagator_queue eventpropagatorqueue = null;

        // constructors
        public pbx2(ipbx_msgpropagator msgpropagator) { this.eventpropagatorqueue = new pbx_eventpropagator_queue(msgpropagator); }
        public pbx2(List<phonenumber> phonenumbers, List<extension> extensions, ipbx_msgpropagator msgpropagator) 
            : this(msgpropagator)
        {
            this.importphonenumbers(phonenumbers);
            this.importextensions(extensions);
        }

        // import a full set of phonenumber lines.
        public void importphonenumbers(List<phonenumber> phonenumbers)
        {
            this.phonenumbers.Clear();
            foreach (var newphonenbr in phonenumbers) { this.phonenumbers.TryAdd(newphonenbr.phonenbr, newphonenbr); }
        }

        public void importextensions(List<extension> extensions)
        {
            this._extensions = new ConcurrentDictionary<string, extension>(extensions.ToDictionary(ext => ext.extension_nbr));
        }

        // create a new phonecall object based on the provided _newcallinfo
        public phonecall callfrominfo(newcallinfo _newcallinfo)
        {
            var newcall = new phonecall()
            {
                switchspecificcallid = this.nextcallid++,
                localnbr = _newcallinfo.localnumber,
                remotenbr = _newcallinfo.remotenumber,
                dialednbr = _newcallinfo.dialednumber,
                starttime = DateTime.Now,
                callstate = phonecall.callstates.ringing
            };

            return newcall;
        }

        // when "newcall" 'interrupt' occurs, track new call, alert phone system via events
        // this is used by the callsimulator as of 10/16/2016... that's right now.. it's pre-Trump.. 
        // umadbro? this.phoneswitchevents is "Add"ed to here
        public phonecall newcallevent(newcallinfo _newcallinfo)
        {
            // make sure the phone number is real. destination phone number may not be
            // the same as dialed phone number. we are referring to the actual destination phone
            // number here. 
            //  e.g. dial 1-800-555-5555 and be connected to 781-555-5555. 
            phonenumber _phonenumber = null;
            try
            {
                _phonenumber = this.phonenumbers.Values.Where(t => t.phonenbr == _newcallinfo.localnumber).Single();
            }
            catch (InvalidOperationException)
            {
                throw new CustomException(CustomExceptionCodes.invalid_local_nbr);
            }

            //// make sure the line is available
            if (_phonenumber.linestate != phonenumber.trunkstates.idle) { throw new CustomException(CustomExceptionCodes.line_busy); }

            updatelinestate(_phonenumber, phonenumber.trunkstates.busy);

            //// create the new call and add to our active calls list
            var newcall = this.callfrominfo(_newcallinfo);
            newcall.connected_extensions.Add(this._extensions["0000"]);
            this.activecalls.Add(newcall);

            // report the new event to any listening clients
            var newcall_eventdetails = new PbxEventArgs_NewCall(this.switchid, newcall);
            this.eventpropagatorqueue.Add(newcall_eventdetails);

            return newcall;
        }

        // return the set of defined phonenumber lines
        public IEnumerable<phonenumber> getallphonenumbers() { return phonenumbers.Values; }

        // get all active calls
        public IEnumerable<phonecall> getallcalls() { return this.activecalls; }

        // will throw on invalid trunkid (invalid_local_nbr)
        public phonenumber getphonelinebynumber(string nbr)
        {
            try { return this.phonenumbers[nbr]; }
            catch (KeyNotFoundException) { throw new CustomException(CustomExceptionCodes.invalid_local_nbr);}
        }

        // return requested phonecall by callid. throw customexception on invalid callid
        public phonecall getcall(int callid)
        {
            try { return this.activecalls.Where(c => c.switchspecificcallid == callid).Single(); }
            catch (InvalidOperationException) { throw new CustomException(CustomExceptionCodes.invalid_call_id);}
        }


        // return requested extension by extension_nbr. throw customexception on invalid extension
        public extension getextension(string extension_nbr)
        {
            try { return this._extensions[extension_nbr];}
            catch (InvalidOperationException) { throw new CustomException(CustomExceptionCodes.invalid_extension);}
        }

        public void update_callstate(int callid, phonecall.callstates newstate)
        {
            lock (lockobject)
            {
                var call = getcall(callid); // will throw on invalid callid
                if (call.callstate != newstate)
                {
                    call.callstate = newstate;
                    var eventdetails = new PbxEventArgs_CallStateChange(callid, newstate);
                    this.eventpropagatorqueue.Add(eventdetails);
                }
            }
        }

        public void answercall(int callid) { update_callstate(callid, phonecall.callstates.active); }
        public void holdcall(int callid) { update_callstate(callid, phonecall.callstates.hold); }
        public void resumecall(int callid) { update_callstate(callid, phonecall.callstates.active); }

        // reset the line state to idle, record the endcallevent, remove from local call list
        public void endcall(int callid)
        {
            lock (lockobject)
            {
                // will throw on invalid callid
                var call = getcall(callid);

                // remove from the local active call cache
                this.activecalls.Remove(call);

                // log the new callended call event
                var eventdetails = new PbxEventArgs_CallEnded(this.switchid, callid, DateTime.Now);
                this.eventpropagatorqueue.Add(eventdetails);

                // if call was valid, phonenumber should be
                var _phoneline = this.phonenumbers[call.localnbr];
                updatelinestate(_phoneline, phonenumber.trunkstates.idle);
            }
        }

        public void connect_extension(int callid, string extension_nbr)
        {
            lock(lockobject)
            {
                var call = getcall(callid);
                if (call.connected_extensions.Where(ext => ext.extension_nbr == extension_nbr).Count() == 0)
                {
                    var ext = getextension(extension_nbr);
                    call.connected_extensions.Add(ext);
                    var eventdetails = new PbxEventArgs_ExtensionConnected(callid, extension_nbr);
                    this.eventpropagatorqueue.Add(eventdetails);
                }
            }
        }

        public void disconnect_extension(int callid, string extension_nbr)
        {
            lock (lockobject)
            {
                var call = getcall(callid);
                var ext2disconnect = call.connected_extensions.Where(ext => ext.extension_nbr == extension_nbr).Single();
                call.connected_extensions.Remove(ext2disconnect);
                var eventdetails = new PbxEventArgs_ExtensionConnected(callid, extension_nbr);
                eventdetails.addremove = "remove";
                this.eventpropagatorqueue.Add(eventdetails);
            }
        }




        private void updatelinestate(phonenumber line2update, phonenumber.trunkstates newstate)
        {
            line2update.linestate = newstate;
            var eventdetails = new PbxEventArgs_TrunkStateChange(line2update.phonenbr, line2update.linestate);
            this.eventpropagatorqueue.Add(eventdetails);
        }

    }

}