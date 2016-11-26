namespace pbx_dto_lib
{
    public class pbx_dto_calltransferred : pbx_dto
    {
        public int callid { get; set; }
        public string from_extension { get; set; }
        public string to_extension { get; set; }
        public pbx_dto_calltransferred(int callid, string from_extension, string to_extension)
        {
            this._dto_type = dto_type.calltransferred;
            this.from_extension = from_extension;
            this.to_extension = to_extension;
        }
        public pbx_dto_calltransferred() { }

    }

}
