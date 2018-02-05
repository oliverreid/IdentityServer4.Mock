using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BookingApi.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Xunit;

namespace BookingApi.IntegrationTest
{
    public class AuthenticationTestsShould
    {
        
        private readonly TestServer _server;
        private readonly HttpClient _client;

        private const string TOKEN =
            "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6IlFVSkVOVFpHUkRoRlFrTkVPRVJFTUVSQk5qZEdPRGRFUXpWR1F6UkVORVpCT1RJek5FWXdSUSJ9.eyJpc3MiOiJodHRwczovL2Rvc3R1ZmYuYXUuYXV0aDAuY29tLyIsInN1YiI6IklpU0RRdEJOMUtBNmlueGtIazE0czRRdlpBdUVabHBYQGNsaWVudHMiLCJhdWQiOiJodHRwczovL2Jvb2tpbmctYXBpLmRvc3R1ZmYiLCJpYXQiOjE1MTc3OTk1ODEsImV4cCI6MTUxNzg4NTk4MSwiYXpwIjoiSWlTRFF0Qk4xS0E2aW54a0hrMTRzNFF2WkF1RVpscFgiLCJndHkiOiJjbGllbnQtY3JlZGVudGlhbHMifQ.P8r9IQaCUFmy9CsITtOA3gL6bW2RzLtHUgjAdPZBmQGI-ds06VY18czaRSGqHgj-watVmleM3z7sZpP_AX36dQGmEXc1pqkBbqyU01Sg2dNUl1QxTipdbU7wladydlrjLXAHvmorkzhyHnlYQa3HM6Klyv39t5kn_YNq2GVK-JdXIKENfxNdyTt7EudArPTVAM7XcB_gEvj0qo8oO5ZRAsgg8BsUWShzTNOSiqwvPRegSXI7u1ewsb4lXFLyn0mpUkKut_Z09gWBn1pe-r5UI7vf3HYsttIid5KfyxI-hZ0HzCqCIHNehzmZyTXAIz1NfZt1V_rSoMBvHjf7pVG4tA";

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
        public async Task RetrieveContentFromSecureEndpointWithToken()
        {
            var request = new HttpRequestMessage() {
                RequestUri = new Uri(_server.BaseAddress + "api/ping/secure"),
                Method = HttpMethod.Get,
            };
            request.Headers.Add("authorization", "Bearer " + TOKEN);
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("Authenticated", responseString);
        }
        
        
        [Fact]
        public async Task RetrieveClaimsFromSecureEndpointWithToken()
        {
            var request = new HttpRequestMessage() {
                RequestUri = new Uri(_server.BaseAddress + "api/claims"),
                Method = HttpMethod.Get,
            };
            request.Headers.Add("authorization", "Bearer " + TOKEN);
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var responseVals = Deserialize(new[] { new {Type = "", Value = ""}}, responseString);
            Assert.True(responseVals.Length > 0);
            Assert.Equal(responseVals.First(v => v.Type == "azp").Value, "IiSDQtBN1KA6inxkHk14s4QvZAuEZlpX");
            
        }

        private T Deserialize<T>(T template, string json) => JsonConvert.DeserializeObject<T>(json);

    }
}
