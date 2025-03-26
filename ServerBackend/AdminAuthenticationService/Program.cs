namespace AdminAuthenticationService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DotNetEnv.Env.Load();

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddAuthorization();

            var app = builder.Build();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
