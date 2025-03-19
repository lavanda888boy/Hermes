using MongoDB.Driver;
using NotificationPreferencesService.Filters;
using NotificationPreferencesService.Models;
using NotificationPreferencesService.Repository;

namespace NotificationPreferencesService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DotNetEnv.Env.Load();

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers(options => options.Filters.Add<ExceptionFilter>());

            var mongoConnectionString = DotNetEnv.Env.GetString("MONGODB_CONNECTION_STRING");
            var mongoDatabaseName = DotNetEnv.Env.GetString("MONGODB_DATABASE");

            builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(mongoConnectionString));
            
            builder.Services.AddScoped(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase(mongoDatabaseName);
            });

            builder.Services.AddScoped<IRepository<NotificationPreference>, NotificationPreferenceRepository>()
                            .AddScoped<IRepository<DeviceTopicInfo>, DeviceTopicInfoRepository>();

            var app = builder.Build();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
