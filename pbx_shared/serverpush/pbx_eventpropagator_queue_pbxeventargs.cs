using pbxlib.dto;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace pbxlib
{
    /* 
     * pbx_eventpropagator_queue - 
     * 
     * store and forward queue for pbx events
     */
    public class pbx_eventpropagator_queue_pbxeventargs
    {
        ipbx_msgpropagator eventpropagator = null;
        private BlockingCollection<PbxEventArgs> switcheventscollection = new BlockingCollection<PbxEventArgs>();

        public pbx_eventpropagator_queue_pbxeventargs(ipbx_msgpropagator _eventpropagator)
        {
            this.eventpropagator = _eventpropagator;
            Thread workerthread = new System.Threading.Thread(event_propagator_q_consumer_loop);
            workerthread.Start();
        }

        // consume events from this.phoneswitchevents
        private void event_propagator_q_consumer_loop()
        {
            // it seems there's really no point in checking the iscompleted flag
            // the Take() call will be interrupted with InvalidOperationException.
            try
            {
                while (true)
                {
                    var evnt = this.switcheventscollection.Take();

                    var dto = args2dto(evnt);
                    if (dto != null)
                    {
                        // forward the event via the injected eventpropagator
                        this.eventpropagator.propagatepbxevent(dto);
                    }
                }

            }catch (InvalidOperationException ex){
                // make sure it's the expected "marked as complete" exception. if not, re-throw.
                if (ex.Message.Contains("marked as complete") == false) { throw; }
            }

        }

        private pbx_dto args2dto(PbxEventArgs args)
        {
            switch (args.eventtype)
            {
                case PbxEventArgs.pbx_eventtypes.callstatechange:
                    return new pbx_dto_callstatechanged((PbxEventArgs_CallStateChange)args);
                case PbxEventArgs.pbx_eventtypes.calltransferred:
                    return new pbx_dto_calltransferred((PbxEventArgs_CallTransfer)args);
                case PbxEventArgs.pbx_eventtypes.endcall:
                    return new pbx_dto_callended((PbxEventArgs_CallEnded)args);
                case PbxEventArgs.pbx_eventtypes.extension_connected:
                    return new pbx_dto_secondary_extension_added((PbxEventArgs_ExtensionConnected)args);
                case PbxEventArgs.pbx_eventtypes.callreceived:
                    return new pbx_dto_callreceived((PbxEventArgs_NewCall)args);
            }

            return null;
        }


        public void Add(PbxEventArgs sea) { this.switcheventscollection.Add(sea);}

    }

}