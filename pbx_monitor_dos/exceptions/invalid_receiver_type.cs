using System;

namespace pbx_monitor_dos.exceptions
{
    class invalid_receiver_type : Exception
    {
        /* constructor */
        public invalid_receiver_type(string run_mode_str) : base("Invalid execute_code: " + run_mode_str)
        {
            this.run_mode_str = run_mode_str;
        }

        public string run_mode_str { get; set; }

    }
}
