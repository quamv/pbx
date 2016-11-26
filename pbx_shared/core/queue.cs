using System.Collections.Generic;

namespace pbx_shared
{
    public class caller_id
    {
        public string id { get; set; }
    }


    public class queue
    {
        public string name { get; set; }

        public string callerid_prefix { get; set; }

        public bool joinwhenempty { get; set; }
        public bool ringinuse { get; set; }
        public int priority { get; set; }
        public int queue_timeout { get; set; }
        public call_route_destination after_timeout { get; set; }

        public enum ring_strategies {  ring_all, round_robin, least_recent, fewest_calls, random }
        ring_strategies ring_strategy { get; set; }
        public int ring_duration { get; set; }

        public enum unanswered_actions { do_nothing };
        public unanswered_actions if_agent_does_not_answer { get; set; } = unanswered_actions.do_nothing;
        public int wrapup_time { get; set; } = 30;
        public bool play_periodic_announcements { get; set; } = false;
        public bool zero_exits_queue { get; set; } = false;
        public bool announce_hold_time { get; set; } = false;
        public bool user_agent_priority { get; set; } = false;
        public bool announce_position { get; set; } = false;

        public enum agent_removal_actions { pause, logoff };
        public agent_removal_actions agent_removal_action { get; set; }

        public List<supportrep> agents { get; set; } = new List<supportrep>();
    }
}
