using MongoDB.Driver;
using Test.API.Controller;

namespace Test.API
{
    public class MongoSetting : Build
    {
        public IMongoDatabase _mongoDb { get; }

        public MongoSetting()
        {
            var client = new MongoClient(_appSettings.MongoDBSetting.ConnectionString);
            _mongoDb = client.GetDatabase(_appSettings.MongoDBSetting.Databases.MediaDatabase.Name);
        }
    }
}