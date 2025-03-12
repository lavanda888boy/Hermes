using MongoDB.Bson.Serialization.Attributes;

namespace NotificationPreferencesService.Models
{
    public class DeviceTopicInfo
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }

        public required string DeviceId { get; set; }

        public required List<string> SubscribedTopics { get; set; }
    }
}
