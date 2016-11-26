using pbx_shared;
using System.Collections.Generic;
using System.ServiceModel;

namespace pbx_web.wcf
{
    [ServiceContract]
    public interface IPhoneSwitchAdminService
    {

        [OperationContract]
        [FaultContract(typeof(FaultBase))]
        IEnumerable<phonenumber> getphonenumbers();

    }

}
