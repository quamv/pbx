using System;

/* pbx_monitor_dos - a dos-based implementation of a pbx event monitor */
namespace pbx_monitor_dos
{
    class App2
    {
        /* the strongly typed msmq queue buffer */
        static pbx_msmq_integration.msmq_pbx_dto_queue pbx_dto_queue;


        /* program entry point */
        static void Main(string[] args)
        {
            Console.WriteLine("MSMQ Queue Name: " + Properties.Settings.Default.queue_name);

            Console.WriteLine("Starting receiver...");

            pbx_dto_queue = new pbx_msmq_integration.msmq_pbx_dto_queue(Properties.Settings.Default.queue_name);

            pbx_dto_queue.start();

            Console.WriteLine("Consuming from main thread...");

            while (true)
            {
                var dto = pbx_dto_queue.Take();

                //pbx_dto_queue.

                pbx_event_handler_dos.event_callback(dto);
            }
        }

    }
}
