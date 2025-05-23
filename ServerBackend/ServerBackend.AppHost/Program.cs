using Projects;

namespace ServerBackend.AppHost;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = DistributedApplication.CreateBuilder(args);

        var notificationPreferences = builder.AddProject<NotificationPreferencesService>("notification-preferences");
        var adminAuthentication = builder.AddProject<AdminAuthenticationService>("admin-auth");

        var gpsStorage = builder.AddRedis("gps-storage")
                                .WithHttpEndpoint(port: 6379, targetPort: 6379)
                                .WithDataVolume("gps-storage-volume");

        var gpsTracking = builder.AddProject<GPSLocationTrackingService>("gps-tracking")
                                 .WithReference(gpsStorage)
                                 .WaitFor(gpsStorage)
                                 .WithReplicas(3);

        var incidentRegistration = builder.AddProject<IncidentRegistrationService>("incident-registration")
                                          .WithReference(gpsStorage)
                                          .WaitFor(gpsStorage)
                                          .WithReplicas(3);

        var apiGateway = builder.AddProject<APIGateway>("api-gateway")
                                .WithReference(notificationPreferences)
                                .WithReference(incidentRegistration)
                                .WithReference(gpsTracking);

        builder.Build().Run();
    }
}