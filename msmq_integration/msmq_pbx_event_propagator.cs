using pbx_shared.dto;
using pbx_shared.serverpush;
using System.Messaging;
using System;

namespace pbx_msmq_integration
{

    /*
     * pbx_msmq_eventpropagator 
     * 
     * forwards phone switch events to a monitor system via MSMQ
     */
    [System.ComponentModel.DesignerCategory("Code")]
    public class msmq_pbx_event_propagator : ipbx_msgpropagator
    {
        public string _host { get; set; }
        public string _queue_name { get; set; }
        public string _queue_path { get { return "FORMATNAME:DIRECT=OS:" + this._host + "\\private$\\" + this._queue_name; } }
        public bool enabled { get; set; }


        /*
         * constructor
         */
        public msmq_pbx_event_propagator(string host, string queue_name)
        {
            this._host = host;
            this._queue_name = queue_name;
            this.enabled = true;
        }


        /* ipbx_msgpropagator implementation */
        public void propagatepbxevent(pbx_dto dto) {
            if (!enabled) { return; }

            sendmsg(new msmq_pbx_dto_message(dto));
        }

        /* attempt to send the msmq message */
        private void sendmsg(Message msg)
        {

            try{
                new MessageQueue(this._queue_path).Send(msg);

            }catch(Exception ex) {
                this.enabled = false;
                throw new Exception("Unable to send to ms message queue: " + this._queue_path + "\n" + ex.Message);

            }
        }
    }



}