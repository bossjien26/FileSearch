using Helpers.MongoDB;

namespace Helpers
{
    public class AppSettings
    {
        public MongoDBSetting MongoDBSetting { get; set; }

        public HeaderSetting HeaderSettings { get; set; }

        public JwtSettings JwtSettings { get; set; }

        public string FilePath { get; set; }
    }
}