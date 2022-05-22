using System.Collections.Generic;
using System.Threading.Tasks;
using Helpers;
using Models.Response;
using MongoDB.Driver;
using MongoEntities;
using Services.Interface;

namespace Services
{
    public class FileToMongoService : IFileToMongoService
    {
        private readonly IMongoCollection<Media> _mediaCollection;

        public FileToMongoService(AppSettings appSettings)
        {
            var client = new MongoClient(appSettings.MongoDBSetting.ConnectionString);
            var database = client.GetDatabase(appSettings.MongoDBSetting.Databases.MediaDatabase.Name);
            _mediaCollection = database.GetCollection<Media>(appSettings.MongoDBSetting.Databases.MediaDatabase.Collections.MediaContentCollectionName);
        }

        public async Task<UploadFileResponse> InsertMediaListToMongo(List<Media> medias)
        {
            var uploadFileResponse = new UploadFileResponse();
            foreach (var media in medias)
            {
                uploadFileResponse.media.Add(await InsertAsync(media));
            }
            return uploadFileResponse;
        }

        public async Task<Media> InsertAsync(Media media)
        {
            await _mediaCollection.InsertOneAsync(media);
            return media;
        }

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