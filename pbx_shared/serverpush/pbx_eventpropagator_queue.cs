using pbx_dto_lib;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace pbx_shared.serverpush
{
    /* 
     * pbx_eventpropagator_queue - 
     * 
     * store and forward queue for pbx events
     */
    public class pbx_eventpropagator_queue
    {
        ipbx_msgpropagator eventpropagator = null;
        private BlockingCollection<pbx_dto> pbx_dto_queue = new BlockingCollection<pbx_dto>();
        

        /* constructor */
        public pbx_eventpropagator_queue(ipbx_msgpropagator _eventpropagator)
        {
            this.eventpropagator = _eventpropagator;
            Thread workerthread = new Thread(event_propagator_q_consumer_loop);
            workerthread.Start();
        }


        
        /* public api to add a new dto to the queue */
        public void Add(pbx_dto dto) { this.pbx_dto_queue.Add(dto); }



        /* loop to consume this.pbx_dto_queue or block */
        private void event_propagator_q_consumer_loop()
        {
            // it seems there's really no point in checking the iscompleted flag
            // the Take() call will be interrupted with InvalidOperationException.
            try
            {
                while (true)
                {
                    var dto = this.pbx_dto_queue.Take();
                    
                    // forward the event via the injected eventpropagator
                    this.eventpropagator.propagatepbxevent(dto);
                }

            }catch (InvalidOperationException ex){
                // make sure it's the expected "marked as complete" exception. if not, re-throw.
                if (ex.Message.Contains("marked as complete") == false) { throw; }
            }

        }


    }

}