using Projects;

namespace ServerBackend.AppHost;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = DistributedApplication.CreateBuilder(args);
        
        builder.AddProject<IncidentRegistrationService>("incident-registration");
        builder.AddProject<GPSLocationTrackingService>("gps-tracking");

        builder.Build().Run();
    }
}