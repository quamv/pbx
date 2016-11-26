namespace pbx_dto_lib
{
    public class pbx_dto_callupdate_base : pbx_dto
    {
        public int callid { get; set; }
        public pbx_dto_callupdate_base(int callid) { this.callid = callid; }
        public pbx_dto_callupdate_base() { }
    }
}
