using pbx_shared.misc;
using System.Collections.Generic;
using System.ServiceModel;


namespace pbx_web.wcf
{
    /* FaultExceptionFactory - helper factory to construct our custom FaultException<FaultBase> objects */
    public class FaultExceptionFactory
    {
        /* create and return a new faultexception<faultbase> for the specified exception code */
        public static FaultException<FaultBase> GetFaultException(CustomExceptionCodes excode)
        {
            FaultBase fault = new FaultBase(excode);
            FaultReason faultReason = new FaultReason(excode.ToString());

            return new FaultException<FaultBase>(fault, faultReason);
        }
    }

}