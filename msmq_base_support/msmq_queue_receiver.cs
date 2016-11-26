using System.Messaging;

namespace msmq_base_support
{
    /*
     * msmq_queue_receiver
     * 
     * basic blind reading/forwarding of shared.msmq messages
     * raises event/calls delegate "message_received" on receipt
     * 
     * injections:
     *  msgreceived_handler - the event callback
     *  formatter - the IMessageFormatter for deserialization
     */
    public class msmq_queue_receiver : msmq_direct_queue
    {
        // callback delegate type definition
        public delegate void msmq_message_received(object sender, Message m);

        // event to raise/callback to call when new message received
        public event msmq_message_received message_received;

        // flag to continue reading messages
        private bool keepreading = false;

        /*
         * constructors
         */
        protected msmq_queue_receiver(string name)
        {
            try
            {
                this._queue = MessageQueue.Create(this.pathfromname(name));
                this._queue.Label = name;
            }
            catch (MessageQueueException ex)
            {
                this._queue = new MessageQueue();
                this._queue.Path = this.pathfromname(name);
                var ex2 = ex;
            }

            this._queue.ReceiveCompleted += new ReceiveCompletedEventHandler(this.queue_msgreceived);
        }
        public msmq_queue_receiver(string name, msmq_message_received msgreceived_handler, IMessageFormatter formatter)
            : this(name)
        {
            this.message_received += msgreceived_handler;
            this._queue.Formatter = formatter;
        }


        // begin the initial read
        public void start()
        {
            this._queue.BeginReceive();
            this.keepreading = true;
        }

        // disable future reads
        public void stop() { this.keepreading = false; }


        // shared.msmq async callback on message received (after a BeginReceive call)
        // read the current shared.msmq message from the queue
        // pass to message_received callback (raise message_received event)
        // on successful processing, call BeginReceive to restart process
        public virtual void queue_msgreceived(object sender, ReceiveCompletedEventArgs e)
        {
            // read the pending message
            MessageQueue mq = (MessageQueue)sender;
            Message m = mq.EndReceive(e.AsyncResult);

            // simply return the message and allow client to parse message
            this.message_received(this, m);

            // restart the receive/process cycle
            if (keepreading)
            {
                this._queue.BeginReceive();
            }
        }

    }

}