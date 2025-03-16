using MongoDB.Bson.Serialization.Attributes;

namespace NotificationPreferencesService.Models
{
    public record NotificationPreference
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; init; }

        public required string PreferenceName { get; init; }

        public required string TopicName { get; init; }

        public required string ImportanceFlag { get; init; }
    }
}
