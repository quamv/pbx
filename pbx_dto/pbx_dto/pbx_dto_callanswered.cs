namespace pbx_dto_lib
{
    public class pbx_dto_callanswered : pbx_dto_callupdate_base
    {
        public pbx_dto_callanswered(int callid) : base(callid)
        {
            this._dto_type = dto_type.callanswered;
        }
        public pbx_dto_callanswered() { }
    }

}
