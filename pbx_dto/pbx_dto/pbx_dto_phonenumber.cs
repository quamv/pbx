namespace pbx_dto_lib
{
    public class pbx_dto_phonenumber
    {
        public readonly object linelock = new object();
        public enum trunktypes { analog, t1 }
        public enum trunkstates { idle, ringing, connected, cleanup, unknown, offline, hold, busy }
        public trunktypes linetype { get; set; }
        public trunkstates linestate { get; set; }
        public string phonenbr { get; set; }

        ////public void applyupdate(phonenumber update)
        ////{
        ////    this.linestate = update.linestate;
        ////}

        //public pbx_dto_phonenumber()
        //{
        //    this.caller_id = "";
        //    this.active = true;
        //    this.caller_id_mode = caller_id_modes.none;
        //    this.caller_id_prefix = "";
        //    this.show_callerid_on_outbound = true;
        //}

        //public bool active { get; set; }
        //public string caller_id { get; set; }
        //public bool show_callerid_on_outbound { get; set; }
        //public enum caller_id_modes { none, your_text, your_text_plus_original }
        //public caller_id_modes caller_id_mode { get; set; }
        //public string caller_id_prefix { get; set; }

        //public call_route_destination default_route { get; set; }
        //public call_route_destination holiday_route { get; set; }

        //public hold_music_group hold_music_group { get; set; }
        //public bool screen_for_privacy { get; set; }

    }
}
