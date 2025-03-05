using MongoDB.Bson.Serialization.Attributes;

namespace NotificationPreferencesService.Models
{
    public class NotificationPreference
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public required string Id { get; set; }

        public required string PreferenceName { get; set; }

        public required string TopicName { get; set; }
    }
}
