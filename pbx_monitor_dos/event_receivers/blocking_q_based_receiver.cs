using System.Threading;

namespace pbx_monitor_dos.event_receivers
{
    class blocking_q_based_receiver
    {
        private void queue_processing_loop()
        {
            var bufferedqueue = new pbx_msmq_integration.blocking_pbx_dto_queue(Properties.Settings.Default.queue_name);

            bufferedqueue.start();

            while (true)
            {
                var dto = bufferedqueue.Take();

                pbx_event_handler_dos.event_callback(dto);
            }

        }

        public void start() { new Thread(queue_processing_loop).Start(); }
    }
}
