using Helpers;
using MongoDB.Driver;

namespace Services
{
    public class MongoSetting
    {
        public IMongoClient _client;
        public MongoSetting(AppSettings appSettings)
        {
            _client = new MongoClient(appSettings.MongoDBSetting.ConnectionString);
        }
    }
}