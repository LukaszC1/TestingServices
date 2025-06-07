using GraphQL.Types;
using GraphqlService.Mutations;
using GraphqlService.Queries;

namespace GraphqlService.Schemas;

public class MainSchema : Schema
{
    public MainSchema(IServiceProvider provider) : base(provider)
    {
        Query = provider.GetRequiredService<MainQuery>();
        Mutation = provider.GetRequiredService<MainMutation>();
    }
}