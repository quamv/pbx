namespace pbx_dto_lib
{
    public class pbx_dto_secondary_extension_removed : pbx_dto_callandextension
    {
        public pbx_dto_secondary_extension_removed() { }
        public pbx_dto_secondary_extension_removed(int callid, string extension_nbr) : base(callid, extension_nbr)
        {
            this._dto_type = dto_type.secondary_extension_removed;
        }
    }

}
