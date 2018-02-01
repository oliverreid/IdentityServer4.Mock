using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BookingApi.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace BookingApi.IntegrationTest
{
    public class AuthenticationTestsShould
    {
        
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public AuthenticationTestsShould()
        {
            
            var cfg = new Dictionary<string, string>
            {
                {"Auth0:Domain", "dostuff.au.auth0.com"},
                {"Auth0:ApiIdentifier", "https://booking-api.dostuff"}
            };
            _server = new TestServer(new WebHostBuilder()
                .ConfigureAppConfiguration(c => c.AddInMemoryCollection(cfg))
                .UseStartup<Startup>());
            _client = _server.CreateClient();
        }
        
        [Fact]
        public async Task RetrieveContentFromUnsecureEndpoint()
        {
            var response = await _client.GetAsync("/api/ping");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("Pong", responseString);
        }
        
        [Fact]
        public async Task NotRetrieveContentFromSecureEndpointWithoutToken()
        {
            var response = await _client.GetAsync("/api/ping/secure");
            
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        
        
        
        [Fact]
        public async Task RetrieveContentFromSecureEndpointWithoken()
        {
            var request = new HttpRequestMessage() {
                RequestUri = new Uri(_server.BaseAddress + "api/ping/secure"),
                Method = HttpMethod.Get,
            };
            request.Headers.Add("authorization", "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6IlFVSkVOVFpHUkRoRlFrTkVPRVJFTUVSQk5qZEdPRGRFUXpWR1F6UkVORVpCT1RJek5FWXdSUSJ9.eyJpc3MiOiJodHRwczovL2Rvc3R1ZmYuYXUuYXV0aDAuY29tLyIsInN1YiI6IklpU0RRdEJOMUtBNmlueGtIazE0czRRdlpBdUVabHBYQGNsaWVudHMiLCJhdWQiOiJodHRwczovL2Jvb2tpbmctYXBpLmRvc3R1ZmYiLCJpYXQiOjE1MTc0NTA3NDEsImV4cCI6MTUxNzUzNzE0MSwiYXpwIjoiSWlTRFF0Qk4xS0E2aW54a0hrMTRzNFF2WkF1RVpscFgiLCJndHkiOiJjbGllbnQtY3JlZGVudGlhbHMifQ.ukPhEqD1iDMCaGaBGiZ56dlXrzoeNOoDswWy8OjaruzJCh2k-vzDcJDnsu0eUAN5AYb13WucdbsB6Z_mbp_YPyOU4E0BzCAZ0SVbFr_SpJUEpDB7kOGCoKVw46UlXv4sWd_eb56ay-6jhbJiB31jTaIsNgtpMbOMUYSBmFU813hrNDLx9wASFnhZd3-8T-I7fyIsmhd5jHfuBjPhV5HdbiphKGBmVGEPDEBgIfNqxd2HxStAMz73SHwySlm3P9oSTfT1LICfFYm8OpCGcpoFfc8_IvFG_esPtv5mJJA7l8UywJ6uQSD6yfmXty5nQXWzczppPrbmPwoPlskZHVnIWA");
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("Authenticated", responseString);
        }
    }
}
