using MongoDB.Bson.Serialization.Attributes;

namespace IncidentRegistrationService.Responses
{
    public record AdminIncidentResponse
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public required string Id { get; init; }

        public required string Category { get; init; }

        public string? Severity { get; init; }

        public double? AreaRadius { get; init; }

        public required DateTimeOffset Timestamp { get; init; }

        public required double Longitude { get; init; }

        public required double Latitude { get; init; }

        public required string UserToReport { get; init; }

        public string? Description { get; init; }
    }
}
