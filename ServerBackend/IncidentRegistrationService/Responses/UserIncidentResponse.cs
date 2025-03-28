using IncidentRegistrationService.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace IncidentRegistrationService.Responses
{
    public record UserIncidentResponse
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public required string Id { get; init; }

        public required string Category { get; init; }

        public required IncidentSeverity Severity { get; init; }

        public required DateTimeOffset Timestamp { get; init; }

        public required string Description { get; init; }
    }
}
