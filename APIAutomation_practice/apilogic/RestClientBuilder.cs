using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace APIAutomation_practice.apilogic
{
    /// <summary>
    /// Builder pattern for configuring and creating RestClient instances
    /// Optimized for parallel execution with connection pooling
    /// </summary>
    public class RestClientBuilder
    {
        private string _baseUrl;
        private TimeSpan _timeout = TimeSpan.FromSeconds(30);
        private int _connectionLimit = 100;
        private SecurityProtocolType _securityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
        private readonly Dictionary<string, string> _defaultHeaders = new Dictionary<string, string>();

        public RestClientBuilder WithBaseUrl(string baseUrl)
        {
            _baseUrl = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl));
            return this;
        }

        public RestClientBuilder WithTimeout(TimeSpan timeout)
        {
            if (timeout.TotalSeconds <= 0)
                throw new ArgumentException("Timeout must be greater than zero", nameof(timeout));
            _timeout = timeout;
            return this;
        }

        public RestClientBuilder WithConnectionLimit(int limit)
        {
            if (limit <= 0)
                throw new ArgumentException("Connection limit must be greater than zero", nameof(limit));
            _connectionLimit = limit;
            return this;
        }

        public RestClientBuilder WithSecurityProtocol(SecurityProtocolType protocol)
        {
            _securityProtocol = protocol;
            return this;
        }

        public RestClientBuilder AddDefaultHeader(string name, string value)
        {
            _defaultHeaders[name ?? throw new ArgumentNullException(nameof(name))] = value;
            return this;
        }

        public RestClientBuilder WithBrowserHeaders()
        {
            AddDefaultHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
            AddDefaultHeader("Accept", "application/json");
            AddDefaultHeader("Accept-Language", "en-US,en;q=0.9");
            AddDefaultHeader("Accept-Encoding", "gzip, deflate, br");
            AddDefaultHeader("Connection", "keep-alive");
            return this;
        }

        public RestClient Build()
        {
            if (string.IsNullOrWhiteSpace(_baseUrl))
                throw new InvalidOperationException("Base URL must be configured before building");

            // Configure security protocol
            ServicePointManager.SecurityProtocol = _securityProtocol;
            ServicePointManager.DefaultConnectionLimit = _connectionLimit;

            var options = new RestClientOptions(_baseUrl)
            {
                Timeout = _timeout
            };

            var client = new RestClient(options);

            // Add default headers to all requests
            foreach (var header in _defaultHeaders)
            {
                client.AddDefaultHeader(header.Key, header.Value);
            }

            return client;
        }
    }
}