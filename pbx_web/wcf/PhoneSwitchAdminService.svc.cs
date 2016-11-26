using pbx_lib;
using pbx_shared;
using System.Collections.Generic;

namespace pbx_web.wcf
{
    // wcf admin interface to a pbx object
    public class PhoneSwitchAdminService : IPhoneSwitchAdminService
    {
        private pbx driver
        { 
            get { return pbx_web.WebApiApplication._pbx; } 
        }

        public IEnumerable<phonenumber> getphonenumbers() { return driver.getallphonenumbers(); }

        //// start/stop functionality removed.. 
        //public void start() { } // safecall(() => driver.start()); }
        //public void stop() { } // safecall(() => driver.stop());}
        //public void disabletrunk(string phonenbr) { safecall(() => driver.resettrunkanddisable(phonenbr));  }
        //public void enabletrunk(string phonenbr) { safecall(() => driver.enabletrunk(phonenbr)); }
        //public void endcall(int callid) { driver.endcall(callid); }

        //void safecall(Action coreaction)
        //{
        //    try{
        //        coreaction();
        //    }catch (pbxlib.CustomException customex){
        //        throw FaultExceptionFactory.GetFaultException(customex.exceptioncode);
        //    } catch (Exception ex) {
        //        throw FaultExceptionFactory.GetFaultException(CustomExceptionCodes.unknown);
        //    }
        //}

    }

}
