using System;
using System.Configuration;

namespace APIAutomation_practice.apilogic
{
    public class ConfigReader
    {
        /// <summary>
        /// Gets the BookStore API URL from configuration
        /// </summary>
        public static string GetBookStoreApiUrl()
        {
            return GetConfigValue("BookStoreApiUrl");
        }

        /// <summary>
        /// Gets the Restful Booker API URL from configuration
        /// </summary>
        public static string GetRestfulBookerApiUrl()
        {
            return GetConfigValue("RestfulBookerApiUrl");
        }

        /// <summary>
        /// Gets the Booking endpoint from configuration
        /// </summary>
        public static string GetBookingEndpoint()
        {
            return GetConfigValue("BookingEndpoint");
        }

        /// <summary>
        /// Gets the request timeout in seconds from configuration
        /// </summary>
        public static int GetRequestTimeout()
        {
            string timeoutValue = GetConfigValue("RequestTimeout");
            return int.TryParse(timeoutValue, out int timeout) ? timeout : 30;
        }

        /// <summary>
        /// Generic method to get any configuration value by key
        /// </summary>
        /// <param name="key">Configuration key</param>
        /// <returns>Configuration value or empty string if not found</returns>
        private static string GetConfigValue(string key)
        {
            try
            {
                string value = ConfigurationManager.AppSettings[key];
                if (string.IsNullOrEmpty(value))
                {
                    Console.WriteLine($"Warning: Configuration key '{key}' not found in App.config");
                    return string.Empty;
                }
                return value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading configuration key '{key}': {ex.Message}");
                return string.Empty;
            }
        }
    }
}