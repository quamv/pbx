using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shared.cytracom
{
    public class call_routing_options
    {
        public call_route_destination try_first_route { get; set; }

        public call_route_destination busy_route { get; set; }

        public call_route_destination not_answered_route { get; set; }

        public call_route_destination offline_route { get; set; }

        public string[] route_type {get;set;}

        public bool blast_calls { get; set; }

        public bool announce_name { get; set; }

        public int num_rings_per_route { get; set; }

        public string unanswered_mailbox { get; set; }
    }
}
