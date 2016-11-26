using pbx_shared;
using pbx_shared.misc;
using System.ServiceModel;

namespace pbx_web.wcf
{
    [ServiceContract]
    public interface IPhoneSwitchPublicAPIService
    {

        [OperationContract]
        [FaultContract(typeof(FaultBase))]
        void endcall(int callid);

        [OperationContract]
        [FaultContract(typeof(FaultBase))]
        phonecall newcall(newcallinfo calldetails);

        [OperationContract]
        [FaultContract(typeof(FaultBase))]
        void connect_extension(int callid, string extension_nbr);

    }

}
