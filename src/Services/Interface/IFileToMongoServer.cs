using System.Threading.Tasks;
using MongoEntities;

namespace Services.Interface
{
    public interface IFileToMongoServer
    {
        Task InsertAsync(Media media);

        Task<Media> GetAsync(string id);

        Task UpdateAsync(string id, Media media);

        Task UpdateToSoftDelete(string id);

        Task RemoveAsync(string id);
    }
}