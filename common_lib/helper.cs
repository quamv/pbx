using System;

namespace common_lib.misc
{
    public class helper
    {

        /* enumstring2value<T> - convert a string to an enum value. throw on invalid */
        public static T enumstring2value<T>(string enumvalasstring)
        {
            return (T)Enum.Parse(typeof(T), enumvalasstring);
        }

        //public static T enumvalue2string2value<T,U>(U initialval)
        //{
        //    var tmp = initialval.ToString();

        //    var result = (T)Enum.Parse(typeof(T), tmp);
        //}


    }
}
