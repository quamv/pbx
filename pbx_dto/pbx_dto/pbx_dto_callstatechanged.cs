namespace pbx_dto_lib
{
    public class pbx_dto_callstatechanged : pbx_dto_callupdate_base
    {
        public pbx_dto_phonecall.callstates newstate { get; set; }

        public pbx_dto_callstatechanged() { }
        public pbx_dto_callstatechanged(int callid) : base(callid)
        {
            this._dto_type = dto_type.callstatechanged;
        }
        public pbx_dto_callstatechanged(int callid, pbx_dto_phonecall.callstates newstate) : base(callid)
        {
            this._dto_type = dto_type.callstatechanged;
            this.newstate = newstate;
        }
    }

}
