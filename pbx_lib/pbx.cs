using pbx_shared;
using pbx_shared.dto;
using pbx_shared.misc;
using pbx_shared.serverpush;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace pbx_lib
{

    /*
     * pbx
     * 
     * the physical hardware where the lines are plugged in
     * raises events based on line/call changes
     * 
     * injections:
     *  ipbx_msgpropagator - responsible for fowarding events
     *  phonenumbers - a list of phonenumbers/lines the pbx is supporting
     */
    [System.ComponentModel.DesignerCategory("Code")]
    public class pbx
    {
        /* the defined phonenumbers/lines on this switch */
        private readonly ConcurrentDictionary<string, phonenumber> phonenumbers = new ConcurrentDictionary<string, phonenumber>();

        /* the known extensions */
        private ConcurrentDictionary<string, pbx_shared.extension> _extensions;

        /* an internally maintained list of 'calls'... maybe should remove and just use pure line state */
        public List<phonecall> activecalls = new List<phonecall>();

        /* each pbx has unique id */
        public int pbxid = 0;

        /* each call has unique id */
        private int nextcallid = 0;

        /* generic object to lock on for thread/client sync */
        private Object lockobject = new Object();

        /* store and forward queue that uses the injected propagator */
        pbx_eventpropagator_queue eventpropagatorqueue = null;



        /* constructors */

        public pbx(ipbx_msgpropagator msgpropagator) { this.eventpropagatorqueue = new pbx_eventpropagator_queue(msgpropagator); }
        public pbx(List<phonenumber> phonenumbers, List<extension> extensions, ipbx_msgpropagator msgpropagator) 
            : this(msgpropagator)
        {
            this.importphonenumbers(phonenumbers);
            this.importextensions(extensions);
        }


        
        /* initializers */

        public void importphonenumbers(List<phonenumber> phonenumbers)
        {
            this.phonenumbers.Clear();
            foreach (var newphonenbr in phonenumbers) { this.phonenumbers.TryAdd(newphonenbr.phonenbr, newphonenbr); }
        }

        public void importextensions(List<extension> extensions)
        {
            this._extensions = new ConcurrentDictionary<string, extension>(extensions.ToDictionary(ext => ext.extension_nbr));
        }



        /* misc */

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



        /* data accessors - throw custom exception on invalid request */

        public pbx_dto_fullstatus getfullstatusdto()
        {
            var dto = new pbx_dto_fullstatus();
            dto.phonenumbers = this.getallphonenumbers().ToList(); // trunks;
            dto.calls = new List<pbx_dto_phonecall>();
            foreach (var nextcall in this.getallcalls().ToList())
            {
                dto.calls.Add(new pbx_dto_phonecall(nextcall));
            }
            dto.timestamp = DateTime.Now;

            return dto;
        }
        
        public IEnumerable<phonenumber> getallphonenumbers() { return phonenumbers.Values; }

        public IEnumerable<phonecall> getallcalls() { return this.activecalls; }

        public phonenumber getphonelinebynumber(string nbr)
        {
            try { return this.phonenumbers[nbr]; }
            catch (KeyNotFoundException) { throw new CustomException(CustomExceptionCodes.invalid_local_nbr);}
        }

        public phonecall getcall(int callid)
        {
            try { return this.activecalls.Where(c => c.switchspecificcallid == callid).Single(); }
            catch (InvalidOperationException) { throw new CustomException(CustomExceptionCodes.invalid_call_id);}
        }

        public extension getextension(string extension_nbr)
        {
            try { return this._extensions[extension_nbr];}
            catch (InvalidOperationException) { throw new CustomException(CustomExceptionCodes.invalid_extension);}
        }




        /* system event processors */

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
            //var calldto = new pbx_dto_phonecall(newcall);
            //var dto = new pbx_dto_callreceived(calldto);
            this.eventpropagatorqueue.Add(new pbx_dto_callreceived(newcall));

            return newcall;
        }

        public void update_callstate(int callid, phonecall.callstates newstate)
        {
            lock (lockobject)
            {
                var call = getcall(callid); // will throw on invalid callid
                if (call.callstate != newstate)
                {
                    call.callstate = newstate;
                    this.eventpropagatorqueue.Add(new pbx_dto_callstatechanged(callid, newstate));
                }
            }
        }

        public void answercall(int callid) { update_callstate(callid, phonecall.callstates.active); }

        public void holdcall(int callid) { update_callstate(callid, phonecall.callstates.hold); }

        public void resumecall(int callid) { update_callstate(callid, phonecall.callstates.active); }

        public void endcall(int callid)
        {
            lock (lockobject)
            {
                // will throw on invalid callid
                var call = getcall(callid);

                // remove from the local active call cache
                this.activecalls.Remove(call);

                // log the new callended call event
                this.eventpropagatorqueue.Add(new pbx_dto_callended(callid));

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
                    this.eventpropagatorqueue.Add(new pbx_dto_secondary_extension_added(callid, extension_nbr));
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
                this.eventpropagatorqueue.Add(new pbx_dto_secondary_extension_removed(callid, extension_nbr));
            }
        }

        private void updatelinestate(phonenumber line2update, phonenumber.trunkstates newstate)
        {
            line2update.linestate = newstate;
            //var eventdetails = new PbxEventArgs_TrunkStateChange(line2update.phonenbr, line2update.linestate);
            //this.eventpropagatorqueue.Add(new pbx_dto_l);
            //this.eventpropagatorqueue.Add(eventdetails);
        }


    }

}