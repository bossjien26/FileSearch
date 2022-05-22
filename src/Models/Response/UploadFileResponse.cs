using System.Collections.Generic;
using MongoEntities;

namespace Models.Response
{
    public class UploadFileResponse
    {
        public List<Media> media { get; set; }
    }
}