using MongoDB.Bson.Serialization.Attributes;

namespace IncidentRegistrationService.Models
{
    public class DeviceTopicInfo
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; init; }

        public required string DeviceId { get; init; }

        public required List<string> SubscribedTopics { get; set; }
    }
}
