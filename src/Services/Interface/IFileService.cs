using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using MongoEntities;

namespace Services.Interface
{
    public interface IFileService
    {
        List<Media> UploadFile(List<IFormFile> formFiles);
    }
}