using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shared.cytracom
{

    class phone_number
    {
        public bool active { get; set; } = true;
        public string caller_id { get; set; } = "";
        public bool show_callerid_on_outbound { get; set; } = false;
        public enum caller_id_modes { none, your_text, your_text_plus_original }
        public caller_id_modes caller_id_mode { get; set; } = caller_id_modes.none;
        public string caller_id_prefix { get; set; } = "";

        public call_route_destination default_route { get; set; }
        public call_route_destination holiday_route { get; set; }

        public hold_music_group hold_music_group { get; set; }
        public bool screen_for_privacy { get; set; }
    }
}
