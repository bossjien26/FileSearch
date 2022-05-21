using System.Threading.Tasks;
using Helpers;
using MongoDB.Driver;
using MongoEntities;
using Services.Interface;

namespace Services
{
    public class FileToMongoServer : IFileToMongoServer
    {
        private readonly IMongoCollection<Media> _mediaCollection;

        public FileToMongoServer(AppSettings appSettings)
        {
            var client = new MongoClient(appSettings.MongoDBSetting.ConnectionString);
            var database = client.GetDatabase(appSettings.MongoDBSetting.Databases.ArticleDatabase.Name);
            _mediaCollection = database.GetCollection<Media>(appSettings.MongoDBSetting.Databases.ArticleDatabase.Collections.ArticleCollectionName);
        }

        public async Task InsertAsync(Media media) =>
            await _mediaCollection.InsertOneAsync(media);

        public async Task<Media> GetAsync(string id) =>
            await _mediaCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task UpdateAsync(string id, Media media) =>
            await _mediaCollection.UpdateOneAsync(x =>
                x.Id == id, Builders<Media>.Update.Set(x => x, media)
            );

        public async Task UpdateToSoftDelete(string id) =>
            await _mediaCollection.UpdateOneAsync(x =>
                x.Id == id, Builders<Media>.Update.Set(x => x.IsDelete, true)
            );

        public async Task RemoveAsync(string id) =>
            await _mediaCollection.DeleteOneAsync(x => x.Id == id);
    }
}