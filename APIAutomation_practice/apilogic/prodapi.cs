using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAutomation_practice.apilogic
{    
    public class prodapi
    {
        public void get_request()
        {


            // Create Rest Client
            var client = new RestClient("https://demoqa.com/BookStore/v1/Books");

            //Create Request
            var request = new RestRequest("", Method.Get);

            //Execute Request
            RestResponse response = client.Execute(request);

            // Print Response
            Console.WriteLine("Status Code: " + response.StatusCode);
            Console.WriteLine("Response Body:");
            Console.WriteLine(response.Content);

            //Validation
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("API Test Passed");
            }
            else
            {
                Console.WriteLine("API Test Failed");
            }

        }

        // Post requests
        public int post_request()
        {
            var client = new RestClient("https://restful-booker.herokuapp.com");

            var request = new RestRequest("/booking", Method.Post);

            var body = new
            {
                firstname = "Thirumala123",
                lastname = "Doe",
                totalprice = 1000,
                depositpaid = true,
                bookingdates = new
                {
                    checkin = "2025-05-01",
                    checkout = "2025-05-10"
                }
            };

            request.AddJsonBody(body);

            var response = client.Execute(request);

            return (int)response.StatusCode;
        }


    }

}
