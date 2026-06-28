using APIAutomation_practice.apilogic;
using APIAutomation_practice.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace APIAutomation_practice
{
    [TestClass]
    public class ApiUnitTestCases
    {
        private prodapi api = new prodapi();

        [TestMethod]
        public void test_api_get_bookings()
        {
            api.get_request();
        }

        [TestMethod]
        [DataTestMethod]
        [DynamicData(nameof(GetBookingTestData), DynamicDataSourceType.Method)]
        public async Task test_api_create_bookings(string jsonBody)
        {
            int statusCode = await api.post_requestAsync(jsonBody); 
            Assert.AreEqual(200, statusCode);
        }
        

        [TestMethod]
        [DataTestMethod]
        [DynamicData(nameof(GetBookingTestData), DynamicDataSourceType.Method)]
        public async Task test_api_create_bookings_Sequential(string jsonBody)
        {
            int statusCode = await api.post_requestAsync(jsonBody);
            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public async Task test_api_create_bookings_Parallel()
        {
            var testDataList = GetBookingTestData().ToList();
            
            // Execute all requests in parallel using shared client
            var tasks = testDataList.Select(data => 
                api.post_requestAsync(data[0] as string)
            ).ToList();

            var results = await Task.WhenAll(tasks);
            
            // Validate all results
            foreach (var statusCode in results)
            {
                Assert.AreEqual(200, statusCode);
            }
        }

        /// <summary>
        /// Provides test data for parameterized tests
        /// </summary>
        public static IEnumerable<object[]> GetBookingTestData()
        {
            return TestDataUtils.GetBookingTestData();
        }
    }
}
