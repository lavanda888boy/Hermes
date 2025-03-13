using Projects;

namespace ServerBackend.AppHost;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = DistributedApplication.CreateBuilder(args);

        var notificationPreferences = builder.AddProject<NotificationPreferencesService>("notification-preferences");
        var incidentRegistration = builder.AddProject<IncidentRegistrationService>("incident-registration");

        var gpsStorage = builder.AddRedis("gps-storage")
                                .WithDataVolume("gps-storage-volume")
                                .WithRedisInsight();

        var gpsTracking = builder.AddProject<GPSLocationTrackingService>("gps-tracking")
                                 .WithReference(gpsStorage)
                                 .WaitFor(gpsStorage)
                                 .WithReplicas(3);

        builder.AddProject<APIGateway>("api-gateway")
                                .WithReference(notificationPreferences)
                                .WithReference(incidentRegistration)
                                .WithReference(gpsTracking);

        builder.Build().Run();
    }
}