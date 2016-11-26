namespace pbx_dto_lib
{
    public class pbx_dto_callreceived : pbx_dto
    {
        public pbx_dto_callreceived() : base() { }
        public pbx_dto_callreceived(pbx_dto_phonecall call_dto) : base()
        {
            this.call_dto = call_dto;
        }

        public pbx_dto_phonecall call_dto { get; set; }

        //public pbx_dto_callreceived(phonecall call) : this(new pbx_dto_phonecall(call)) { }

    }

}
