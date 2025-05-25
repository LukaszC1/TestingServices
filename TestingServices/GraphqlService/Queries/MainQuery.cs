using GraphQL;
using GraphQL.Types;
using GraphqlService.Types;
using LocalRepository;
using LocalRepository.DTO;

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

        Field<BooleanGraphType>("addCustomer")
            .Arguments(new QueryArguments(new QueryArgument<NonNullGraphType<CustomerInputType>> { Name = "customer" }))    
            .ResolveAsync(async context =>
            {
                var customer = context.GetArgument<Customer>("customer");
                return await repository.AddCustomerAsync(customer);
            });

        Field<ListGraphType<EmployeeType>>("employees")
            .ResolveAsync(async context => await repository.GetAllEmployeesAsync());

        Field<IntGraphType>("addOrder")
            .Description("Adds a new order and returns the order ID.")
            .Arguments()
            .ResolveAsync(async context => 
            {
                var order = context.GetArgument<Order>("order");
                return await repository.AddOrderAsync(order);
            });
    }
}