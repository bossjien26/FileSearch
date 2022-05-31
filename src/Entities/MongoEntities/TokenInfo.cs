using System;
using Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoEntities
{
    public class TokenInfo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string GroupId { get; set; }

        public string Project { get; set; }

        public string Password { get; set; }

        public string Token { get; set; }

        public RoleEnum Role { get; set; } = RoleEnum.Customer;

        public string createAt { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();

        public string ExpireAt { get; set; } = DateTimeOffset.UtcNow.AddYears(10).ToUnixTimeMilliseconds().ToString();
    }
}