namespace GPSLocationTrackingService.Models
{
    public record GPSLocation
    {
        public required double Longitude { get; init; }

        public required double Latitude { get; init; }
    }
}
