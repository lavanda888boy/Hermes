namespace IncidentRegistrationService.DTOs
{
    public record UserIncidentRequest
    {
        public required string Category { get; init; }

        public required string UserToReport { get; init; }

        public required string Description { get; init; }
    }
}
