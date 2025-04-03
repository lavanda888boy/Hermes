using MongoDB.Bson.Serialization.Attributes;

namespace IncidentRegistrationService.DTOs
{
    public record AdminIncidentRequest
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; init; }

        public required string Category { get; init; }

        public required string Severity { get; init; }

        public required double AreaRadius { get; init; }

        public required double Longitude { get; init; }

        public required double Latitude { get; init; }

        public required string Description { get; init; }
    }
}
