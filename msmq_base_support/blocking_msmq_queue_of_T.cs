using System;
using System.Collections.Concurrent;
using System.Messaging;

namespace msmq_base_support
{
    public class blocking_msmq_queue_of_t<T> : iBlockingReadonlyQueue<T>
    {
        /* a blocking queue of msmq messages */
        private blocking_msmq_queue msmq_buffer;

        /* an injected msmq message parsing function */
        private Func<Message, T> msg_parsing_func;


        /* constructors */

        /* constructor - Type array provided. default to XmlMessageFormatter */
        public blocking_msmq_queue_of_t(string name, iQueueableType<T> queueable_type)
            : this(name, queueable_type.supportedtypes, queueable_type.parse_msmq_message) { }
        public blocking_msmq_queue_of_t(string name, Type[] supported_tpes, Func<Message, T> msgparser_param)
            : this(name, new XmlMessageFormatter(supported_tpes), msgparser_param) { }

        /* constructor - IMessageFormatter provided */
        public blocking_msmq_queue_of_t(string name, IMessageFormatter formatter, Func<Message, T> msgparser_param)
        {
            this.msg_parsing_func = msgparser_param;
            this.msmq_buffer = new blocking_msmq_queue(name, formatter);
            this._queue = new BlockingCollection<T>();
        }

        /* our private underlying blockingcollection */
        public BlockingCollection<T> _queue;

        /* pass-through call to BlockingCollection<T>.Take() */
        public T Take() { return this._queue.Take(); }


        /* start the underlying queue and our processing thread */
        public void start()
        {
            this.msmq_buffer.start();
            new System.Threading.Thread(message_processing_loop).Start();
        }

        /* loop to process incoming messages from our feeder queue */
        public void message_processing_loop()
        {
            while (true)
            {
                // take raw msmq messages from our feeder queue
                var newmsg = this.msmq_buffer.Take();

                // parse the message into a typed object using the injected parser
                var typedobj = msg_parsing_func(newmsg);

                // add to meself
                this._queue.Add(typedobj);
            }
        }
    }
}
