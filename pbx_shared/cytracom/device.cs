using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shared.cytracom
{
    class device
    {
        public string mac { get; set; }
        public string device_template { get; set; }
        public string device_type { get; set; }
        public mailbox mailbox { get; set; }
        public string timezone { get; set; }
        public extension extension { get; set; }
    }
}
