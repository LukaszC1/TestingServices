using LocalRepository;

namespace RestService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSingleton<IRepository, LocalRepository.LocalRepository>();
            builder.Services.AddAuthorization();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            // REST endpoints reflecting gRPC/CoreWCF
            app.MapGet("/products/{id:int}", async (int id, IRepository repo) =>
            {
                var product = await repo.GetProductByIdAsync(id);
                return product is not null ? Results.Ok(product) : Results.NotFound();
            });

            app.MapGet("/products", async (IRepository repo) =>
            {
                var products = await repo.GetAllProductsAsync();
                return Results.Ok(products);
            });

            app.MapGet("/customers/{id}", async (string id, IRepository repo) =>
            {
                var customer = await repo.GetCustomerByIdAsync(id);
                return customer is not null ? Results.Ok(customer) : Results.NotFound();
            });

            app.MapGet("/customers", async (IRepository repo) =>
            {
                var customers = await repo.GetAllCustomersAsync();
                return Results.Ok(customers);
            });

            app.MapGet("/orders/{id:int}", async (int id, IRepository repo) =>
            {
                var order = await repo.GetOrderByIdAsync(id);
                return order is not null ? Results.Ok(order) : Results.NotFound();
            });

            app.MapGet("/orders", async (IRepository repo) =>
            {
                var orders = await repo.GetAllOrdersAsync();
                return Results.Ok(orders);
            });

            app.MapGet("/orders/{id:int}/details", async (int id, IRepository repo) =>
            {
                var details = await repo.GetOrderDetailsByOrderIdAsync(id);
                return Results.Ok(details);
            });

            app.Run();
        }
    }
}
