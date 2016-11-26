namespace pbx_shared
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

        public mailbox unanswered_mailbox { get; set; }
    }
}
