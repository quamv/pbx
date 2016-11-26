using pbx_lib;
using pbx_shared;
using pbx_shared.misc;
using System;

namespace pbx_web.wcf
{


    public class PhoneSwitchPublicAPIService : IPhoneSwitchPublicAPIService
    {
        private pbx _pbx
        {
            get { return pbx_web.WebApiApplication._pbx; }
        }

        public void connect_extension(int callid, string extension_nbr)
        {
            try {
                _pbx.connect_extension(callid, extension_nbr);

            } catch (CustomException customex) {
                throw FaultExceptionFactory.GetFaultException(customex.exceptioncode);

            } catch (Exception) {
                throw FaultExceptionFactory.GetFaultException((CustomExceptionCodes)0);
            }
        }

        public void endcall(int callid)
        {
            try{
                _pbx.endcall(callid);

            } catch (CustomException customex) {
                throw FaultExceptionFactory.GetFaultException(customex.exceptioncode);

            } catch (Exception) {
                throw FaultExceptionFactory.GetFaultException((CustomExceptionCodes)0);
            }
        }


        // create a new phonecall object based on the provided _newcallinfo
        public phonecall callfrominfo(newcallinfo _newcallinfo)
        {
            var newcall = new phonecall()
            {
                switchspecificcallid = 100,
                localnbr = _newcallinfo.localnumber,
                remotenbr = _newcallinfo.remotenumber,
                dialednbr = _newcallinfo.dialednumber,
                starttime = DateTime.Now,
                callstate = phonecall.callstates.ringing
            };

            return newcall;
        }


        public phonecall newcall(newcallinfo calldetails)
        {
            try {
                return _pbx.newcallevent(calldetails);

            }catch (CustomException customex) {
                throw FaultExceptionFactory.GetFaultException(customex.exceptioncode);

            } catch (Exception){
                throw FaultExceptionFactory.GetFaultException((CustomExceptionCodes)0);
            }
        }



    }


}
