using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace APIAutomation_practice.apilogic
{
    public class prodapi
    {
        // Shared RestClient instance for all requests (enables connection pooling)
        private static readonly Lazy<RestClient> _bookStoreClient = new Lazy<RestClient>(() =>
            new RestClientBuilder()
                .WithBaseUrl(ConfigReader.GetBookStoreApiUrl())
                .WithConnectionLimit(100)
                .Build()
        );

        private static readonly Lazy<RestClient> _bookingClient = new Lazy<RestClient>(() =>
            new RestClientBuilder()
                .WithBaseUrl(ConfigReader.GetRestfulBookerApiUrl())
                .WithTimeout(TimeSpan.FromSeconds(ConfigReader.GetRequestTimeout()))
                .WithConnectionLimit(100)
                .WithBrowserHeaders()
                .Build()
        );

        public void get_request()
        {
            try
            {
                var request = new RestRequest("", Method.Get);
                RestResponse response = _bookStoreClient.Value.Execute(request);

                Console.WriteLine("Status Code: " + response.StatusCode);
                Console.WriteLine("Response Body:");
                Console.WriteLine(response.Content);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine("API Test Passed");
                }
                else
                {
                    Console.WriteLine("API Test Failed");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GET request: " + ex.Message);
            }
        }

        public async Task<int> post_requestAsync(object bookingData)
        {
            try
            {
                var request = new RestRequest(ConfigReader.GetBookingEndpoint(), Method.Post);
                request.AddJsonBody(bookingData);

                RestResponse response = await _bookingClient.Value.ExecuteAsync(request);

                Console.WriteLine("Status Code: " + response.StatusCode);
                Console.WriteLine("Response Body: " + response.Content);

                if (!response.IsSuccessful)
                {
                    Console.WriteLine("Error Message: " + response.ErrorMessage);
                    Console.WriteLine("Exception: " + response.ErrorException?.Message);
                }

                return (int)response.StatusCode;
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine("Request timeout: " + ex.Message);
                return 408;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("HTTP request failed: " + ex.Message);
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
                return 0;
            }
        }
    }
}
