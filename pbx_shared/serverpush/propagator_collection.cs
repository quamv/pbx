using pbx_shared.dto;
using System.Collections.Generic;
using System;

namespace pbx_shared.serverpush
{
    /*
     * propagator_collection 
     * 
     * a collection of phone switch message propagators to send event notifications to
     * various clients.
     */
    public class propagator_collection : ipbx_msgpropagator
    {
        public List<ipbx_msgpropagator> propagators = new List<ipbx_msgpropagator>();

        public bool enabled { get; set; }

        public void propagatepbxevent(pbx_dto dto)
        {
            foreach (var gator in propagators) { gator.propagatepbxevent(dto); }
        }

    }
}