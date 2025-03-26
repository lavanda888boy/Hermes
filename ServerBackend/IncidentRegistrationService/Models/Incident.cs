using MongoDB.Bson.Serialization.Attributes;

namespace IncidentRegistrationService.Models
{
    public class Incident
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; init; }

        public required string Category { get; set; }

        public IncidentSeverity? IncidentSeverity { get; set; }

        public required DateTimeOffset Timestamp { get; set; }

        public required double Longitude { get; set; }

        public required double Latitude { get; set; }

        public required string UserToReport { get; init; }

        public string? Status { get; set; }

        public string? Description { get; set; }
    }
}
