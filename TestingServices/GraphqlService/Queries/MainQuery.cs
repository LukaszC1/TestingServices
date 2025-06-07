using GraphQL;
using GraphQL.Types;
using GraphqlService.Types;
using LocalRepository;

namespace GraphqlService.Queries;

public sealed class MainQuery : ObjectGraphType
{
    public MainQuery(IRepository repository)
    {
        Field<ListGraphType<ProductType>>("products")
            .ResolveAsync(async context =>
                await repository.GetAllProductsAsync());

        Field<CustomerType>("customer")
            .Arguments(new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id" })
            .ResolveAsync(async context =>
                await repository.GetCustomerByIdAsync(context.GetArgument<string>("id")));

        Field<ListGraphType<CustomerType>>("customers")
            .ResolveAsync(async context =>
                await repository.GetAllCustomersAsync());

        Field<ListGraphType<OrderType>>("orders")
            .ResolveAsync(async context =>
                await repository.GetAllOrdersAsync());

        Field<ListGraphType<OrderDetailType>>("orderDetails")
            .Arguments(new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "orderId" })
            .ResolveAsync(async context =>
                await repository.GetOrderDetailsByOrderIdAsync(context.GetArgument<int>("orderId")));

        Field<ListGraphType<EmployeeType>>("employees")
            .ResolveAsync(async context => await repository.GetAllEmployeesAsync());

        Field<ListGraphType<OrderWithDetailsType>>("ordersWithDetails")
            .Description("Returns orders with their order details. Optionally filter by orderId.")
            .Arguments(new QueryArgument<IntGraphType> { Name = "orderId", Description = "Optional order ID" })
            .ResolveAsync(async context =>
            {
                var orderId = context.GetArgument<int?>("orderId");
                return await repository.GetOrdersWithDetailsAsync(orderId);
            });

        Field<ListGraphType<CustomerWithOrdersType>>("customersWithOrders")
            .Description("Returns customers with their orders and order details. Optionally filter by customerId.")
            .Arguments(new QueryArgument<StringGraphType> { Name = "customerId", Description = "Optional customer ID" })
            .ResolveAsync(async context =>
            {
                var customerId = context.GetArgument<string>("customerId");
                return await repository.GetCustomerWithOrdersAsync(customerId);
            });
    }
}