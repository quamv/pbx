using System;

namespace pbx_shared.misc
{
    /* application defined custom exceptions */
    public enum CustomExceptionCodes
    {
        unknown, invalid_local_nbr, invalid_call_id, line_busy, system_unavailable, invalid_extension
    }

    public class CustomException : Exception
    {
        public CustomExceptionCodes exceptioncode;

        public CustomException(CustomExceptionCodes code) { this.exceptioncode = code; }
    }


}
