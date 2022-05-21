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

        public bool IsDownload { get; set; }

        public bool IsDelete { get; set; } = false;

        public DateTime CreateAt { get; set; } = DateTime.Now;

        public DateTime UpdateAt { get; set; } = DateTime.Now;
    }
}