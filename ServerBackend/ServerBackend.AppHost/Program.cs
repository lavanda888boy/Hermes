using Projects;
using System.Security.Cryptography.Xml;

namespace ServerBackend.AppHost;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = DistributedApplication.CreateBuilder(args);

        var notificationPreferences = builder.AddProject<NotificationPreferencesService>("notification-preferences");
        var incidentRegistration = builder.AddProject<IncidentRegistrationService>("incident-registration");
        var gpsTracking = builder.AddProject<GPSLocationTrackingService>("gps-tracking");

        builder.AddProject<APIGateway>("api-gateway")
                                .WithReference(notificationPreferences)
                                .WithReference(incidentRegistration)
                                .WithReference(gpsTracking);

        builder.Build().Run();
    }
}