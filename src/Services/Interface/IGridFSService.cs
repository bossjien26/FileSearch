using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MongoDB.Driver.GridFS;

namespace Services.Interface
{
    public interface IGridFSService
    {
        Task<MongoDB.Bson.ObjectId> UploadFile(string fileName, byte[] source);

        Task<byte[]> Download(MongoDB.Bson.ObjectId id);

        Task<MemoryStream> DownloadToStream(MongoDB.Bson.ObjectId id);

        Task DownloadFromStream(MongoDB.Bson.ObjectId id);

        Task<GridFSFileInfo> FindFile(string id);

        Task<List<GridFSFileInfo>> FindFileList(string fileName);
    }
}