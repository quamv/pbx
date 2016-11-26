namespace pbx_dto_lib
{

    public class pbx_dto_secondary_extension_added : pbx_dto_callandextension
    {
        public pbx_dto_secondary_extension_added() { }
        public pbx_dto_secondary_extension_added(int callid, string extension_nbr) : base(callid, extension_nbr)
        {
            this._dto_type = dto_type.secondary_extension_added;
        }
    }

}
