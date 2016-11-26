namespace pbx_dto_lib
{
    public class pbx_dto_callended : pbx_dto_callupdate_base
    {
        public pbx_dto_callended() { }
        public pbx_dto_callended(int callid) : base(callid)
        {
            this._dto_type = dto_type.callended;
        }
    }

}
