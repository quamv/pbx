using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxlib
{
    public class call_state_change_dto
    {
        public int callid { get; set; }
        public shared.core.phonecall.callstates newstate { get; set; }
    }
}
