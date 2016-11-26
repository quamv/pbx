using System.Collections.Concurrent;
using System.Messaging;

namespace msmq_base_support
{
    /* abstracts the receiving process into a standard blocking queue interface */
    public class blocking_msmq_queue : BlockingCollection<Message>
    {
        /* constructor */
        public blocking_msmq_queue(string queuename, IMessageFormatter formatter)
        {
            this.rcvr = new msmq_queue_receiver(queuename, this.newmsg, formatter);
        }

        /* basic msmq queue reading functionality */
        msmq_queue_receiver rcvr;

        /* start the receiver, begin accepting message notifications */
        public void start() { this.rcvr.start(); }


        /* whenever the receiver receives, we add the new element */
        private void newmsg(object sender, Message m) { this.Add(m); }


    }
}
