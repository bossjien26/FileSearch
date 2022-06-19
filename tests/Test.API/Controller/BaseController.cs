using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using api;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

namespace Test.API.Controller
{
    public class BaseController
    {
        private string _token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJtYWlsIjoiamFuQGV4YW1wbGUuY29tIiwicGFzc3dvcmQiOiJhYWFhYWFhIiwibmJmIjoxNjMzNzQ5MzkzLCJleHAiOjE2MzQzNTQxOTMsImlhdCI6MTYzMzc0OTM5M30.SA82v2dOOgMAVy8RR_3yit2ETF_Frx3nhghnA0ZYFN8";

        protected string _testMail = "";

        private readonly WebApplicationFactory<Startup> _factory;

        protected HttpClient _httpClient;

        public BaseController()
        {
            _factory = new WebApplicationFactory<Startup>();
            _httpClient = _factory.WithWebHostBuilder(builder =>
                { }).CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false,
                });

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }

        protected StringContent PostType(object request)
        {
            return new StringContent(JsonConvert.SerializeObject(request).ToString(),
            Encoding.UTF8, "application/json");
        }
    }
}