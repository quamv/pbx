using System;

namespace pbx_dto_lib
{
    public class pbx_dto
    {
        public enum dto_type
        {
            callreceived,
            callanswered,
            calltransferred,
            callstatechanged,
            callended,
            secondary_extension_added,
            secondary_extension_removed,
            unknown
        }

        public dto_type _dto_type { get; set; }

        public DateTime createdate { get; set; }

        public pbx_dto() { this.createdate = DateTime.Now; }
    }

}
