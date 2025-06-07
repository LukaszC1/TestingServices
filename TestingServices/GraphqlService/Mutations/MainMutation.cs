using GraphQL;
using GraphQL.Types;
using GraphqlService.Types;
using LocalRepository;
using LocalRepository.DTO;

namespace GraphqlService.Mutations;

public sealed class MainMutation : ObjectGraphType
{
    public MainMutation(IRepository repository)
    {
        Field<BooleanGraphType>("addCustomer")
        .Arguments(new QueryArguments(new QueryArgument<NonNullGraphType<CustomerInputType>> { Name = "customer" }))    
        .ResolveAsync(async context =>
        {
            var customer = context.GetArgument<Customer>("customer");
            return await repository.AddCustomerAsync(customer);
        });

        Field<IntGraphType>("addOrder")
        .Description("Adds a new order and returns the order ID.")
            .Arguments(new QueryArgument<NonNullGraphType<OrderInputType>> { Name = "order" })
        .ResolveAsync(async context =>
        {
            var order = context.GetArgument<Order>("order");
            return await repository.AddOrderAsync(order);
        });
    }
}

