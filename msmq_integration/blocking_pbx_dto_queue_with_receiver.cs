using pbx_shared.dto;
using System.Collections.Concurrent;
using System.Messaging;

namespace pbx_msmq_integration
{
    public class blocking_pbx_dto_queue_with_receiver : BlockingCollection<pbx_dto>
    {
        receiver rcvr;

        // whenever the receiver receives, we add the new element
        private void myhandler(object sender, pbx_dto dto)
        {
            this.Add(dto);
        }

        public blocking_pbx_dto_queue_with_receiver(string queuename)
        {
            this.rcvr = new receiver(queuename, this.myhandler);

            this.rcvr.start();
        }
    }
}
