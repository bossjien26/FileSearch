using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Response;
using MongoEntities;

namespace Services.Interface
{
    public interface IFileToMongoService
    {
        Task<UploadFileResponse> InsertMediaListToMongo(List<Media> medias);

        Task<Media> InsertAsync(Media media);

        Task<Media> GetAsync(string id);

        Task UpdateAsync(string id, Media media);

        Task UpdateToSoftDelete(string id);

        Task RemoveAsync(string id);
    }
}