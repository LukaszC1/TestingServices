using LocalRepository;
using LocalRepository.DTO;

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

            app.MapGet("/employees", async (IRepository repo) =>
            {
                var employees = await repo.GetAllEmployeesAsync();
                return Results.Ok(employees);
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

            app.MapGet("/orders-with-details", async (IRepository repo, int? orderId) =>
            {
                var orders = await repo.GetOrdersWithDetailsAsync(orderId);
                return Results.Ok(orders);
            });

            app.MapGet("/customers-with-orders", async (IRepository repo, string? customerId) =>
            {
                var customers = await repo.GetCustomerWithOrdersAsync(customerId);
                return Results.Ok(customers);
            });

            app.MapPost("/customers", async (Customer customer, IRepository repo) =>
            {
                var result = await repo.AddCustomerAsync(customer);
                return result ? Results.Created($"/customers/{customer.CustomerID}", customer) : Results.BadRequest();
            });

            app.MapPost("/orders", async (Order order, IRepository repo) =>
            {
                var orderId = await repo.AddOrderAsync(order);
                return orderId > 0 ? Results.Created($"/orders/{orderId}", orderId) : Results.BadRequest();
            });

            app.Run();
        }
    }
}
