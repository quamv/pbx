using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shared.cytracom
{
    public class ip_endpoint
    {
        public System.Net.IPAddress ipaddress { get; set; }

        public ip_endpoint(System.Net.IPAddress ipaddress)
        {
            this.ipaddress = ipaddress;
        }
    }
}
