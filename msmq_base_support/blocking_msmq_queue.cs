using System.Collections.Concurrent;
using System.Messaging;

namespace msmq_base_support
{
    /* abstracts the receiving process into a standard blocking queue interface */
    public class blocking_msmq_queue :  iBlockingReadonlyQueue<Message>
    {
        /* constructor */
        public blocking_msmq_queue(string queuename, IMessageFormatter formatter)
        {
            this.rcvr = new msmq_queue_receiver(queuename, this.newmsg, formatter);
            this._queue = new BlockingCollection<Message>();
        }

        /* basic msmq queue reading functionality */
        msmq_queue_receiver rcvr;

        /* whenever the receiver receives, we add the new element */
        private void newmsg(object sender, Message m) { this._queue.Add(m); }

        /* start the receiver, begin accepting message notifications */
        public void start() { this.rcvr.start(); }


        /* our internal buffer */
        BlockingCollection<Message> _queue;

        /* pass-through */
        public Message Take() { return _queue.Take(); }



    }
}
