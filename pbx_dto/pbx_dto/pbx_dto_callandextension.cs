namespace pbx_dto_lib
{

    public class pbx_dto_callandextension : pbx_dto_callupdate_base
    {
        public string extension_nbr { get; set; }

        public pbx_dto_callandextension() { }
        public pbx_dto_callandextension(int callid, string extension_nbr) : base(callid) { this.extension_nbr = extension_nbr; }
    }

}
