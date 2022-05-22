using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoEntities
{
    public class Media
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string OriginName { get; set; }

        public string NewFileName { get; set; }

        public string Content { get; set; }

        public bool IsDownload { get; set; } = true;

        public bool IsDelete { get; set; } = false;

        public string CreateAt { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();

        public string UpdateAt { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
    }
}