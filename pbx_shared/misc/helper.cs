using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbx_shared.misc
{
    public class helper
    {

        /* enumstring2value<T> - convert a string to an enum value. throw on invalid */
        public static T enumstring2value<T>(string enumvalasstring)
        {
            return (T)Enum.Parse(typeof(T), enumvalasstring);
        }


    }
}
