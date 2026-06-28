using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace APIAutomation_practice.Utils
{
    /// <summary>
    /// Utility class for handling test data from CSV files
    /// </summary>
    public static class TestDataUtils
    {
        /// <summary>
        /// Reads CSV file and yields test data as JSON strings for parameterized tests
        /// Supports nested objects using "/" notation (e.g., "bookingdates/checkin")
        /// </summary>
        public static IEnumerable<object[]> GetBookingTestData()
        {
            var csvFilePath = "C://temp//testdata.csv";

            var lines = File.ReadAllLines(csvFilePath);

            if (lines.Length < 2)
            {
                yield break;
            }

            var headers = lines[0].Split(',');

            var result = new List<Dictionary<string, object>>();

            foreach (var line in lines.Skip(1))
            {
                var values = line.Split(',');

                var rootObject = new Dictionary<string, object>();

                for (int i = 0; i < headers.Length; i++)
                {
                    string header = headers[i];
                    string value = i < values.Length ? values[i] : "";

                    // ✅ Handle nested (bookingdates/checkin)
                    if (header.Contains("/"))
                    {
                        var parts = header.Split('/');
                        var parent = parts[0];
                        var child = parts[1];

                        if (!rootObject.ContainsKey(parent))
                        {
                            rootObject[parent] = new Dictionary<string, object>();
                        }

                        var nestedDict = (Dictionary<string, object>)rootObject[parent];
                        nestedDict[child] = ConvertValue(value);
                    }
                    else
                    {
                        rootObject[header] = ConvertValue(value);
                    }
                }

                result.Add(rootObject);
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var jsonBody = JsonSerializer.Serialize(rootObject, options);
                Console.WriteLine(jsonBody);
                yield return new object[] { jsonBody };
            }

            //var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            //var jsonBody = JsonSerializer.Serialize(result, options);
            //Console.WriteLine(jsonBody);
            //yield return new object[] { jsonBody };
        }

        /// <summary>
        /// Converts CSV values to appropriate types
        /// </summary>
        private static object ConvertValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            value = value.Trim();

            // Try to parse as boolean
            if (bool.TryParse(value, out bool boolValue))
                return boolValue;

            // Try to parse as integer
            if (int.TryParse(value, out int intValue))
                return intValue;

            // Try to parse as decimal
            if (decimal.TryParse(value, out decimal decimalValue))
                return decimalValue;

            // Return as string
            return value;
        }

        
       

    }

  
    public class BookingTestDataRow
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TotalPrice { get; set; }
        public bool DepositPaid { get; set; }
        public string CheckIn { get; set; }
        public string CheckOut { get; set; }
        public string AdditionalNeeds { get; set; }
    }
}