using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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

            builder.Services.AddControllers();

            builder.Services.AddSingleton(FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("hermes-firebase-adminsdk.json")
            }));

            var app = builder.Build();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
