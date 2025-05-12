using MongoDB.Bson.Serialization.Attributes;

namespace AdminAuthenticationService.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; init; }

        public required string UserName { get; set; }

        public required string Password { get; set; }
    }
}
