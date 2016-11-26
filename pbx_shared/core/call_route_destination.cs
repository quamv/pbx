namespace pbx_shared
{
    public class call_route_destination
    {
        public enum routable_types { extension, queue, attendant, mailbox, find_me }
        public routable_types route_destination_type { get; set; } = routable_types.extension;
        public int ring_time { get; set; }
        public extension target_extension { get; set; }
        public queue target_queue { get; set; }
        public attendant target_attendant { get; set; }
        public mailbox target_mailbox { get; set; }
    }
}
