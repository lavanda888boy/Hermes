
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace NotificationPreferencesService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddSingleton(FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("hermes-firebase-adminsdk.json")
            }));

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
