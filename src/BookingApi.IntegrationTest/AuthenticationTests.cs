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

        private const string TOKEN =
            "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6IlFVSkVOVFpHUkRoRlFrTkVPRVJFTUVSQk5qZEdPRGRFUXpWR1F6UkVORVpCT1RJek5FWXdSUSJ9.eyJpc3MiOiJodHRwczovL2Rvc3R1ZmYuYXUuYXV0aDAuY29tLyIsInN1YiI6IklpU0RRdEJOMUtBNmlueGtIazE0czRRdlpBdUVabHBYQGNsaWVudHMiLCJhdWQiOiJodHRwczovL2Jvb2tpbmctYXBpLmRvc3R1ZmYiLCJpYXQiOjE1MTc2OTgwMzQsImV4cCI6MTUxNzc4NDQzNCwiYXpwIjoiSWlTRFF0Qk4xS0E2aW54a0hrMTRzNFF2WkF1RVpscFgiLCJndHkiOiJjbGllbnQtY3JlZGVudGlhbHMifQ.tSG3ONjzDJSJYmNmFy1zyH8AkVxm3ue93721qeigYVjP3r_mPvKDIwlh_JJp4qiarXtw_hPfcajSzEKPFJYESj9HkD-MkPvj3U-e4z7U13lEIXVuthvo4-Sj9Cpnk5RqoPHzwM4UPglKPqK-3g4Uy62RpEW7a3-OQlvD8wBwp0uLLqkIvBfceM0_E1gqAiV6moO-7IuVHSx_wiLXpLX1_AlnbjpKp3-C4D2J4fij0KdyeVZTrCGY3Ov_hNH21CfYZtpd444CYSDJ9LxOVFCZhDY115OkvuxAadlF-9zoyMpf1tQmx_CvwI9TfubjKnlQAMBRjCGlp-eynnnq2Mt0TA";

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
        }
    }
}
