using Helpers;
using Microsoft.Extensions.Configuration;

namespace Test.API
{
    public class Build
    {
        public AppSettings _appSettings;

        public Build()
        {
            Startup();
        }

        public IConfigurationRoot GetIConfigurationRoot()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();
        }

        public void Startup()
        {
            var config = GetIConfigurationRoot();
            _appSettings = config.GetSection(nameof(AppSettings)).Get<AppSettings>();
        }
    }
}