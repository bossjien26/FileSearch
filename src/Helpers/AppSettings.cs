using Helpers.MongoDB;

namespace Helpers
{
    public class AppSettings
    {
        public MongoDBSetting MongoDBSetting { get; set; }

        public string FilePath { get; set; }
    }
}