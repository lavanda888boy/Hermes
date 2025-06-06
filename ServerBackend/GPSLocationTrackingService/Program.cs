using GPSLocationTrackingService.Filters;

namespace GPSLocationTrackingService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers(options => options.Filters.Add<ExceptionFilter>());

            builder.AddRedisClient(connectionName: "gps-storage");

            var app = builder.Build();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
