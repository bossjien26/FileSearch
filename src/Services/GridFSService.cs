using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Helpers;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Services.Interface;

namespace Services
{
    public class GridFSService : IGridFSService
    {
        private IGridFSBucket _bucket;

        public GridFSService(AppSettings appSettings)
        {
            var client = new MongoClient(appSettings.MongoDBSetting.ConnectionString);
            var database = client.GetDatabase(appSettings.MongoDBSetting.Databases.MediaDatabase.Name);
            _bucket = new GridFSBucket(database);
        }

        public async Task<MongoDB.Bson.ObjectId> UploadFile(string fileName, byte[] source)
        {
            return await _bucket.UploadFromBytesAsync(fileName, source);
        }

        public async Task<byte[]> Download(MongoDB.Bson.ObjectId id)
        {
            return await _bucket.DownloadAsBytesAsync(id);
        }

        public async Task<MemoryStream> DownloadToStream(MongoDB.Bson.ObjectId id)
        {
            var ms = new MemoryStream();
            await _bucket.DownloadToStreamAsync(id, ms);
            return ms;
        }

        public async Task DownloadFromStream(MongoDB.Bson.ObjectId id)
        {
            using (var stream = await _bucket.OpenDownloadStreamAsync(id))
            {
                // read from stream until end of file is reached
                await stream.CloseAsync();
            }
        }

        public async Task<GridFSFileInfo> FindFile(string id)
        {
            ObjectId gridfsObjectID = new ObjectId(id);

            var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", gridfsObjectID);
            // var sort = Builders<GridFSFileInfo>.Sort.Descending(x => x.UploadDateTime);
            // var options = new GridFSFindOptions
            // {
            //     Limit = 1,
            //     Sort = sort
            // };
            using (var cursor = await _bucket.FindAsync(filter))
            {
                return await cursor.FirstOrDefaultAsync();
            }
        }

        public async Task<List<GridFSFileInfo>> FindFileList(string fileName)
        {
            var filter = Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename, fileName);
            // var sort = Builders<GridFSFileInfo>.Sort.Descending(x => x.UploadDateTime);
            // var options = new GridFSFindOptions
            // {
            //     Limit = 1,
            //     Sort = sort
            // };
            using (var cursor = await _bucket.FindAsync(filter))
            {
                return await cursor.ToListAsync();
            }
        }
    }
}