using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using IncidentRegistrationService.Filters;
using IncidentRegistrationService.Hubs;
using IncidentRegistrationService.Models;
using IncidentRegistrationService.Repository;
using IncidentRegistrationService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;

namespace IncidentRegistrationService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DotNetEnv.Env.Load();

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                            .AddJwtBearer(options =>
                            {
                                options.Events = new JwtBearerEvents
                                {
                                    OnMessageReceived = context =>
                                    {
                                        var accessToken = context.Request.Query["access_token"];

                                        var path = context.HttpContext.Request.Path;

                                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/incidentHub"))
                                        {
                                            context.Token = accessToken;
                                        }

                                        return Task.CompletedTask;
                                    }
                                };

                                options.TokenValidationParameters = new TokenValidationParameters
                                {
                                    ValidateIssuer = true,
                                    ValidateAudience = true,
                                    ValidateLifetime = true,
                                    ValidateIssuerSigningKey = true,
                                    ValidIssuer = DotNetEnv.Env.GetString("JWT_ISSUER"),
                                    ValidAudience = DotNetEnv.Env.GetString("JWT_AUDIENCE"),
                                    IssuerSigningKey = new SymmetricSecurityKey(
                                        Encoding.ASCII.GetBytes(DotNetEnv.Env.GetString("JWT_SECRET"))
                                    )
                                };
                            });

            builder.Services.AddControllers(options => options.Filters.Add<ExceptionFilter>());

            var mongoConnectionString = DotNetEnv.Env.GetString("MONGODB_CONNECTION_STRING");
            var mongoDatabaseName = DotNetEnv.Env.GetString("MONGODB_DATABASE");

            builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(mongoConnectionString));

            builder.Services.AddScoped(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase(mongoDatabaseName);
            });

            builder.Services.AddSingleton(FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("hermes-firebase-adminsdk.json")
            }));

            builder.AddRedisClient(connectionName: "gps-storage");

            builder.Services.AddSignalR();

            builder.Services.AddScoped<IIncidentCorrelationService, IncidentCorrelationService>();
            builder.Services.AddScoped<INotificationTransmissionService, NotificationTransmissionService>();
            
            builder.Services.AddScoped<IRepository<Incident>, IncidentRepository>();
            builder.Services.AddScoped<IRepository<DeviceTopicInfo>, DeviceTopicInfoRepository>();
            builder.Services.AddScoped<IRepository<NotificationPreference>, NotificationPreferenceRepository>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontendAccess", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });

            var app = builder.Build();

            app.UseWebSockets();

            app.UseCors("AllowFrontendAccess");

            app.UseAuthorization();

            app.MapHub<IncidentHub>("/incidentHub");

            app.MapControllers();

            app.Run();
        }
    }
}
