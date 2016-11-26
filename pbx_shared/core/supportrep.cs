using System;

namespace pbx_shared
{
    interface iphonecallrecipient
    {
        void takecall(phonecall call);
    }

    public class supportrep : iphonecallrecipient
    {
        public string repid { get; set; }

        public enum repstates
        {
            offduty
            ,
            available
                ,
            inacall
                ,
            postcallcleanup
                , unavailable
        }

        /* time to auto-transition from cleanup to available */
        public DateTime returntoavailabletime { get; set; }

        public repstates repstate { get; set; }

        public phonecall activecall
        {
            get;
            set;
        }

        public bool standby { get; set; }

        public void resettoavailable()
        {
            this.activecall = null;
            this.repstate = repstates.available;
        }

        public void takecall(phonecall call)
        {
            this.activecall = call;
        }
    }

}
