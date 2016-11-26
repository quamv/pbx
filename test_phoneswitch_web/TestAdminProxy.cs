using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.ServiceModel;
//using phoneswitchlib;
using test_phoneswitch_web.adminproxy;


namespace test_phoneswitch_web
{
    [TestClass]
    public class TestAdminProxy
    {

        adminproxy.PhoneSwitchAdminServiceClient adminproxy =
            new adminproxy.PhoneSwitchAdminServiceClient();

        [TestMethod]
        public IEnumerable<phonenumber> getphonenumbers()
        {
            var phonenumbers = adminproxy.getphonenumbers();

            Assert.IsTrue(phonenumbers.Length > 0);

            return phonenumbers;
        }

        //[TestMethod]
        //public void disabletrunk_validtrunkid() {
        //    safecall(() => adminproxy.disabletrunk("1234567890"));
        //    safecall(() => adminproxy.disabletrunk("1234567891"));
        //    safecall(() => adminproxy.disabletrunk("1234567892"));
        //    safecall(() => adminproxy.disabletrunk("1234567893"));
        //    safecall(() => adminproxy.disabletrunk("1234567894"));
        //    safecall(() => adminproxy.disabletrunk("1234567895"));
        //}

        //[TestMethod]
        //public void disabletrunk_invalidlocalnbr()
        //{

        //    expectedfaultexception(
        //        () => adminproxy.disabletrunk(""), // the primary call, which we expect to fail
        //        (ex) => Assert.AreEqual(ex.Message, "invalid local nbr")); // the expected failure

        //    expectedfaultexception(
        //        () => adminproxy.disabletrunk(" "), // the primary call, which we expect to fail
        //        (ex) => Assert.AreEqual(ex.Message, "invalid local nbr")); // the expected failure

        //    expectedfaultexception(
        //        () => adminproxy.disabletrunk("          "), // the primary call, which we expect to fail
        //        (ex) => Assert.AreEqual(ex.Message, "invalid local nbr")); // the expected failure
            
        //    expectedfaultexception(
        //        () => adminproxy.disabletrunk("11231232"), // the primary call, which we expect to fail
        //        (ex) => Assert.AreEqual(ex.Message, "invalid local nbr")); // the expected failure
            
        //    expectedfaultexception(
        //        () => adminproxy.disabletrunk("x234567890"), // the primary call, which we expect to fail
        //        (ex) => Assert.AreEqual(ex.Message, "invalid local nbr")); // the expected failure

        //    expectedfaultexception(
        //        () => adminproxy.disabletrunk("12345678901"), // the primary call, which we expect to fail
        //        (ex) => Assert.AreEqual(ex.Message, "invalid local nbr")); // the expected failure

        //}

        //[TestMethod]
        //public void enabletrunk_validtrunkid() {
        //    safecall(() => adminproxy.enabletrunk("1234567890"));
        //    safecall(() => adminproxy.enabletrunk("1234567891"));
        //    safecall(() => adminproxy.enabletrunk("1234567892"));
        //    safecall(() => adminproxy.enabletrunk("1234567893"));
        //    safecall(() => adminproxy.enabletrunk("1234567894"));
        //    safecall(() => adminproxy.enabletrunk("1234567895"));
        //    safecall(() => adminproxy.enabletrunk("1234567896")); 
        //}

        //[TestMethod]
        //public void enabletrunk_invalidlocalnbr()
        //{
        //    expectedfaultexception(
        //        () => adminproxy.enabletrunk("1"), // the primary call, which we expect to fail
        //        (ex) => Assert.AreEqual(ex.Message, "invalid local nbr")); // the expected failure

        //    expectedfaultexception(
        //        () => adminproxy.enabletrunk("123456789"), // the primary call, which we expect to fail
        //        (ex) => Assert.AreEqual(ex.Message, "invalid local nbr")); // the expected failure

        //    expectedfaultexception(
        //        () => adminproxy.enabletrunk("xasdf"), // the primary call, which we expect to fail
        //        (ex) => Assert.AreEqual(ex.Message, "invalid local nbr")); // the expected failure

        //    expectedfaultexception(
        //        () => adminproxy.enabletrunk(""), // the primary call, which we expect to fail
        //        (ex) => Assert.AreEqual(ex.Message, "invalid local nbr")); // the expected failure

        //    expectedfaultexception(
        //        () => adminproxy.enabletrunk("aaaa#@#$f3"), // the primary call, which we expect to fail
        //        (ex) => Assert.AreEqual(ex.Message, "invalid local nbr")); // the expected failure
        //}

        //public void faultdetails(FaultException<FaultBase> faultexception)
        //{
        //    Console.WriteLine("FaultException Received");
        //    Console.WriteLine("Ex Code: " + faultexception.Detail.excode);
        //}

        ////[TestMethod]
        ////public void start() { safecall(() => adminproxy.start()); }

        ////[TestMethod]
        ////public void stop() { safecall(() => adminproxy.stop()); }

        //// wrapper to verify an action that is expected to fail with a particular type of faultexception
        //void expectedfaultexception(Action primaryaction, Action<FaultException> comparisonaction)
        //{
        //    try{
        //        // the primary action, expects a faultexception
        //        primaryaction(); 
        //    }
        //    catch (FaultException<FaultBase> ex) {
        //        // verify the exception details via the passed in delegate
        //        comparisonaction(ex);
        //        return;
        //    }

        //    Assert.Fail("Did not receive expected fault exception.");
        //}

        //// standard wrapper to call a delegate and catch faultexception or exception
        //void safecall(Action lambdahere)
        //{
        //    try
        //    {
        //        lambdahere();
        //    }
        //    catch (FaultException<FaultBase> ex) { faultdetails(ex); Assert.Fail(ex.Message); }
        //    catch (Exception ex) { Assert.Fail(ex.Message); }
        //}

    }


}
