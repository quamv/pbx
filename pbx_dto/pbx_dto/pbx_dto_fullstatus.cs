using System;
using System.Collections.Generic;

namespace pbx_dto_lib
{

    public class pbx_dto_fullstatus
    {
        public List<pbx_dto_phonenumber> phonenumbers;
        public List<pbx_dto_phonecall> calls;
        public DateTime timestamp;

        public pbx_dto_fullstatus()
        {

        }

    }
}
