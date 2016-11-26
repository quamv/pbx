using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shared.cytracom
{
    public class extension
    {
        public string extension_nbr { get; set; }

        public string extension_name { get; set; }

        public string outbound_callerid_phonenbr { get; set; }

        public string e911location { get; set; }

        public call_routing_options routing_options { get; set; }

        public enum extension_types { digital, ip };

        public extension_types extension_type { get; set; }

    }
}
