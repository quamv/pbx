using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using test_pbx_web2.publicapiproxy;
using System.ServiceModel;
using System.Messaging;

namespace test_pbx_web2
{
    public class pots_co
    {
        publicapiproxy.PhoneSwitchPublicAPIServiceClient publicapiproxy =
            new publicapiproxy.PhoneSwitchPublicAPIServiceClient();

        adminproxy.PhoneSwitchAdminServiceClient _adminproxy =
            new adminproxy.PhoneSwitchAdminServiceClient();

        public List<adminproxy.phonenumber> pbx_phonenumbers = new List<adminproxy.phonenumber>();
        public List<adminproxy.phonenumber> available_nbrs
        {
            get
            {
                return this.pbx_phonenumbers
                        .Where(t => t.linestate == adminproxy.phonenumber.trunkstates.idle)
                        .ToList();
            }
        }

        public pots_co()
        {
            foreach (var pbxtrunk in Properties.Settings.Default.pbx_trunks)
            {
                var newtrunk = new adminproxy.phonenumber();
                var pbxtrunk_fields = pbxtrunk.Split(',');
                newtrunk.phonenbr = pbxtrunk_fields[0];
                this.pbx_phonenumbers.Add(newtrunk);
            }

        }

        public adminproxy.phonenumber getavailableline()
        {
            var tmp = _adminproxy.getphonenumbers()
                    .Where(pn => pn.linestate == adminproxy.phonenumber.trunkstates.idle)
                    .First();

            return tmp;

            //var nextline = this.available_nbrs.First();
            //nextline.linestate = adminproxy.phonenumber.trunkstates.busy;
            //return nextline;
        }

        public void returntrunk(string phonenbr)
        {
            var line = this.pbx_phonenumbers.Where(t => t.phonenbr == phonenbr).Single();
            line.linestate = adminproxy.phonenumber.trunkstates.idle;
        }
    }


    [TestClass]
    public class TestPublicProxy
    {
        publicapiproxy.PhoneSwitchPublicAPIServiceClient publicapiproxy =
            new publicapiproxy.PhoneSwitchPublicAPIServiceClient();

        adminproxy.PhoneSwitchAdminServiceClient adminproxy =
            new adminproxy.PhoneSwitchAdminServiceClient();



        [TestMethod]
        public void test1()
        {
            var pots = new pots_co();
            var newcallconnectioninfo = new newcallinfo()
            {
                dialednumber = "1800FUCKYOU",
                localnumber = pots.getavailableline().phonenbr,
                //localnumber = "1234567890",// pots.getavailableline().phonenbr,
                remotenumber = "2342352349"
            };

            var tmp5 = publicapiproxy.newcall(newcallconnectioninfo);
            Assert.IsNotNull(tmp5.switchspecificcallid);
            Assert.IsTrue(tmp5.switchspecificcallid > -1);
            //Assert.AreEqual(tmp5.switchspecificcallid, 1);

            Console.WriteLine("Callid = " + tmp5.switchspecificcallid);

        }


        [TestMethod]
        public void newcall_withvalidparameters()
        {
            var newcallconnectioninfo = new newcallinfo()
            {
                dialednumber = "1800FUCKYOU",
                localnumber = "1234567890",
                remotenumber = "2342352349"
            };

            try
            {
                var newcall = publicapiproxy.newcall(newcallconnectioninfo);
                Assert.IsNotNull(newcall.switchspecificcallid);
                Console.WriteLine("Callid = " + newcall.switchspecificcallid);
            }
            catch (FaultException)
            {
                Assert.Fail("Generic fault exception caught");
            }

        }

        [TestMethod]
        public void newcall_withnodialednumber()
        {
            var newcallconnectioninfo = new newcallinfo()
            {
                localnumber = "1234567890",
                remotenumber = "2342352349"
            };

            var newcall = publicapiproxy.newcall(newcallconnectioninfo);

            Assert.IsNotNull(newcall.switchspecificcallid);

            Console.WriteLine("Callid = " + newcall.switchspecificcallid);
        }

        [TestMethod]
        public void call_alllines()
        {
            try
            {
                while (true)
                {
                    newcall_connect();
                }
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "Sequence contains no elements");
                //Console.WriteLine(ex.Message);
            }

            //var pots = new pots_co();

            //while (pots.available_nbrs.Count() > 0)
            //{
            //    var tmp = pots.getavailableline();
            //    var newcallconnectioninfo = new newcallinfo()
            //    {
            //        dialednumber = "1800FUCKYOU",
            //        localnumber = tmp.phonenbr,
            //        remotenumber = "2342352349"
            //    };

            //    Exception expected_exception = null;
            //    try
            //    {
            //        var newcall = publicapiproxy.newcall(newcallconnectioninfo);
            //        //Assert.Fail("expected to throw exception on call to newcall() with bad localnbr field");
            //    }
            //    catch (FaultException<FaultBase> ex) { Assert.Fail(ex.Message); }
            //    catch (Exception ex) { Assert.Fail(ex.Message); }

            //}

            //var call2fail = new newcallinfo() {
            //    dialednumber = "1800FUCKYOU"
            //    ,localnumber = pots.pbx_phonenumbers[0].phonenbr
            //    ,remotenumber = "55555511111"
            //};


            //FaultException<FaultBase> expected_faultexception = null;
            //try
            //{
            //    var newcall2 = publicapiproxy.newcall(call2fail);
            //    Assert.Fail("expected \"line busy\" exception not received");
            //}
            //catch (FaultException<FaultBase> ex)
            //{
            //    expected_faultexception = ex;
            //    //Console.WriteLine("Received expected FaultException<FaultBase>");
            //}
            //catch (Exception ex)
            //{
            //    Assert.Fail("Default exception caught... Expected generic FaultException<FaultBase>");
            //    //expected_exception = ex;
            //}

            //Assert.IsNotNull(expected_faultexception);
            //Assert.IsTrue(expected_faultexception.Message.Contains("busy"));

        }

        [TestMethod]
        public void newcall_badlocalnumber()
        {
            var newcallconnectioninfo = new newcallinfo()
            {
                dialednumber = "1800FUCKYOU",
                localnumber = "x234567890",
                remotenumber = "2342352349"
            };

            Exception expected_exception = null;
            try
            {
                var newcall = publicapiproxy.newcall(newcallconnectioninfo);
                Assert.Fail("expected to throw exception on call to newcall() with bad localnbr field");
            }
            catch (FaultException<FaultBase>)
            {
                Console.WriteLine("Received expected FaultException<FaultBase>");
            }
            catch (Exception ex)
            {
                Assert.Fail("Default exception caught... Expected generic FaultException<FaultBase>");
                expected_exception = ex;
            }

        }



        [TestMethod]
        public void newcall_connect()
        {
            var pots = new pots_co();

            var newcallconnectioninfo = new newcallinfo()
            {
                dialednumber = "1800FUCKYOU",
                localnumber = pots.getavailableline().phonenbr,
                remotenumber = "2342352349"
            };

            var newcall = publicapiproxy.newcall(newcallconnectioninfo);

            Assert.IsNotNull(newcall.switchspecificcallid);

            Console.WriteLine("Callid = " + newcall.switchspecificcallid);
        }

        [TestMethod]
        public void newcall_connectnextopenline()
        {
            var pots = new pots_co();

            var newcallconnectioninfo = new newcallinfo()
            {
                dialednumber = "1800FUCKYOU",
                localnumber = pots.getavailableline().phonenbr,
                remotenumber = "2342352349"
            };


            var newcall = publicapiproxy.newcall(newcallconnectioninfo);

            Assert.IsNotNull(newcall.switchspecificcallid);

            Console.WriteLine("Callid = " + newcall.switchspecificcallid);
        }

        [TestMethod]
        public void newcall_connect_thenbusy()
        {
            var newcallconnectioninfo = new newcallinfo()
            {
                dialednumber = "1800FUCKYOU",
                localnumber = "1234567890",
                remotenumber = "2342352349"
            };

            var newcall = publicapiproxy.newcall(newcallconnectioninfo);

            Assert.IsNotNull(newcall.switchspecificcallid);

            Console.WriteLine("Callid = " + newcall.switchspecificcallid);
            FaultException<FaultBase> expected_faultexception = null;
            try
            {
                var newcall2 = publicapiproxy.newcall(newcallconnectioninfo);
                Assert.Fail("expected \"line busy\" exception not received");
            }
            catch (FaultException<FaultBase> ex)
            {
                expected_faultexception = ex;
                //Console.WriteLine("Received expected FaultException<FaultBase>");
            }
            catch (Exception)
            {
                Assert.Fail("Default exception caught... Expected generic FaultException<FaultBase>");
                //expected_exception = ex;
            }

            Assert.IsNotNull(expected_faultexception);
            Assert.IsTrue(expected_faultexception.Message.Contains("busy"));
        }

        [TestMethod]
        public void connectanddisconnect()
        {



            var newcallconnectioninfo = new newcallinfo()
            {
                dialednumber = "1800FUCKYOU",
                localnumber = "1234567890",
                remotenumber = "2342352349"
            };

            var newcall = publicapiproxy.newcall(newcallconnectioninfo);

            Assert.IsNotNull(newcall.switchspecificcallid);

            Console.WriteLine("Callid = " + newcall.switchspecificcallid);

            publicapiproxy.endcall(newcall.switchspecificcallid);

            Console.WriteLine("After the disconnect call.");
        }


    } // class testpublicproxy



}
