using AdminAuthenticationService.Filters;
using AdminAuthenticationService.Models;
using AdminAuthenticationService.Repository;
using MongoDB.Driver;

namespace AdminAuthenticationService
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

            builder.Services.AddScoped<IRepository<User>, UserRepository>();

            builder.Services.AddAuthorization();

            var app = builder.Build();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
