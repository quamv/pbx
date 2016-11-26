using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using test_pbx_web2.LIVE_publicapiproxy;

namespace test_pbx_web2
{
    [TestClass]
    public class pbx_web_iis_test
    {
        [TestMethod]
        public void TestMethod1()
        {
            var tst = new LIVE_adminproxy.PhoneSwitchAdminServiceClient();

            var nbrs = tst.getphonenumbers();

            Assert.AreEqual(nbrs.Length, 20);
        }

        [TestMethod]
        public void live_newcall_connect()
        {
            var pots = new pots_co();

            var newcallconnectioninfo = new newcallinfo()
            {
                dialednumber = "1800FUCKYOU",
                localnumber = pots.getavailableline().phonenbr,
                remotenumber = "2342352349"
            };

            var publicapiproxy = new LIVE_publicapiproxy.PhoneSwitchPublicAPIServiceClient();
            var newcall = publicapiproxy.newcall(newcallconnectioninfo);

            Assert.IsNotNull(newcall.switchspecificcallid);

            Console.WriteLine("Callid = " + newcall.switchspecificcallid);
        }

    }
}
