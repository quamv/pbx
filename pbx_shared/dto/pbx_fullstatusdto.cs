using shared.core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace pbxlib
{
    /*
     * pbx_fullstatusdto
     * 
     * dto containing the full switch status info, including
     *   - all trunks and their state
     *   - a current timestamp
     */
    public class pbx_fullstatusdto
    {
        public List<phonenumber> phonenumbers;
        public List<pbx_dto_phonecall> calls;
        public DateTime timestamp;

        public pbx_fullstatusdto(pbx switch_driver)
        {
            this.phonenumbers = switch_driver.getallphonenumbers().ToList(); // trunks;
            this.calls = new List<pbx_dto_phonecall>();
            foreach (var nextcall in switch_driver.getallcalls().ToList())
            {
                this.calls.Add(new pbx_dto_phonecall(nextcall));
            }
            this.timestamp = DateTime.Now;
        }
    }


}
