using GraphQL;
using GraphQL.Types;
using LocalRepository;

public sealed class MainQuery : ObjectGraphType
{
    public MainQuery(IRepository repository)
    {
        Field<ProductType>("product")
            .Arguments(new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" })
            .ResolveAsync(async context =>
                await repository.GetProductByIdAsync(context.GetArgument<int>("id")));

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

        Field<OrderType>("order")
            .Arguments(new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" })
            .ResolveAsync(async context =>
                await repository.GetOrderByIdAsync(context.GetArgument<int>("id")));

        Field<ListGraphType<OrderType>>("orders")
            .ResolveAsync(async context =>
                await repository.GetAllOrdersAsync());

        Field<ListGraphType<OrderDetailType>>("orderDetails")
            .Arguments(new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "orderId" })
            .ResolveAsync(async context =>
                await repository.GetOrderDetailsByOrderIdAsync(context.GetArgument<int>("orderId")));
    }
}