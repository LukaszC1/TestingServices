using GraphQL.Types;
using GraphqlService.Queries;

namespace GraphqlService.Schemas;

public class MainSchema : Schema
{
    public MainSchema(IServiceProvider provider) : base(provider)
    {
        Query = provider.GetRequiredService<MainQuery>();
    }
}