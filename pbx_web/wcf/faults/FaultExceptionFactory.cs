using pbx_shared.misc;
using System.Collections.Generic;
using System.ServiceModel;


namespace pbx_web.wcf
{
    /* FaultExceptionFactory - helper factory to construct our custom FaultException<FaultBase> objects */
    public class FaultExceptionFactory
    {
        /* the dictionary of custom exception code strings */
        public static Dictionary<CustomExceptionCodes, string> codetostring = new Dictionary<CustomExceptionCodes, string>()
        {
            {CustomExceptionCodes.unknown, "unknown"}
            ,{CustomExceptionCodes.invalid_call_id, "invalid call id"}
            ,{CustomExceptionCodes.invalid_local_nbr, "invalid local nbr"}
            ,{CustomExceptionCodes.line_busy, "line busy"}
            ,{CustomExceptionCodes.system_unavailable, "system unavailable"}
        };

        /* create and return a new faultexception<faultbase> for the specified exception code */
        public static FaultException<FaultBase> GetFaultException(CustomExceptionCodes excode)
        {
            FaultBase fault = new FaultBase(excode);
            FaultReason faultReason = new FaultReason(codetostring[excode]);
            return new FaultException<FaultBase>(fault, faultReason);
        }
    }

}