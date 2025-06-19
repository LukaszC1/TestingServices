using GrpcService.Client;
using LocalRepository;

namespace GrpcService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddSingleton<IRepository>(sp => new LocalRepository.LocalRepository(connectionString));
            builder.Services.AddGrpc();
            builder.Services.AddGrpcClient<GrpcServiceClient>(o =>
            {
                o.Address = new Uri("https://localhost:5001");
            });

            builder.WebHost.ConfigureKestrel(options =>
            {
                var http2 = options.Limits.Http2;
                http2.InitialConnectionWindowSize = 1024 * 1024 * 2; // 2 MB
                http2.InitialStreamWindowSize = 1024 * 1024; // 1 MB
            });
            
           var app = builder.Build();
           app.UseWebSockets();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<Services.GrpcService>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}