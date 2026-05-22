using APIAutomation_practice.apilogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System;


namespace APIAutomation_practice
{
    [TestClass]
    public class ApiUnitTestCases
    {
        prodapi api= new prodapi();
        [TestMethod]
        public void test_api_get_bookings()
        {
            api.get_request();
        }

        [TestMethod]
        public void test_api_create_bookings()
        {            

            Assert.AreEqual(200, api.post_request());
        }

    }
}
