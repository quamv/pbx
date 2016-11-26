namespace msmq_base_support
{
    /*
     * direct_queue
     * 
     * a base form of MSMQ queue
     */
    public class msmq_direct_queue
    {
        public string queue_name { get; set; }
        public string pathfromname(string name) { return @".\private$\" + name; }
        public System.Messaging.MessageQueue _queue;
    }
}

