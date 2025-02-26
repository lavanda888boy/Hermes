namespace APIGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.AddServiceDefaults();

            builder.Services.AddReverseProxy()
                            .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
                            .AddServiceDiscoveryDestinationResolver();

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.MapReverseProxy();

            app.Run();
        }
    }
}
