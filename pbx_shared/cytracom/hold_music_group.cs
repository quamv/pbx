using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shared.cytracom
{

    public class music_track {
        public string name { get; set; }
        public int size { get; set; }
        public int volume_adjust { get; set; }
        public string actions { get; set; }
        public string file_path { get; set; }
    }

    class hold_music_group
    {
        public string name { get; set; }
        public bool use_for_outbound { get; set; } = false;
        public bool shuffle_tracks { get; set; } = false;

        List<music_track> tracks { get; set; } = new List<music_track>();
    }
}
