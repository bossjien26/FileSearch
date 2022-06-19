using System.Net.Http;
using System.Net.Http.Headers;
using api;
using Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Test.API.Controller
{
    public class Build
    {
        private string _token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJtYWlsIjoiamFuQGV4YW1wbGUuY29tIiwicGFzc3dvcmQiOiJhYWFhYWFhIiwibmJmIjoxNjMzNzQ5MzkzLCJleHAiOjE2MzQzNTQxOTMsImlhdCI6MTYzMzc0OTM5M30.SA82v2dOOgMAVy8RR_3yit2ETF_Frx3nhghnA0ZYFN8";

        protected string _testMail = "";

        private readonly WebApplicationFactory<Startup> _factory;

        protected HttpClient _httpClient;

        public AppSettings _appSettings;

        public Build()
        {
            _factory = new WebApplicationFactory<Startup>().
                WithWebHostBuilder(builder =>
                {
                    builder.ConfigureAppConfiguration((context, config) =>
                    {
                        config.AddJsonFile("appsettings.Test.json")
                        .AddEnvironmentVariables();
                    });
                    builder.ConfigureServices((builderContext, service) =>
                    {
                        _appSettings = builderContext.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
                        // services.Configuration<AppSettings>(Configaio)
                        // service.AddSingleton()
                        // services.AddSingleton(appSettings);
                    });
                });
            // _appSettings = _factory.GetSection(nameof(AppSettings)).Get<AppSettings>();
            _httpClient = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }
    }
}