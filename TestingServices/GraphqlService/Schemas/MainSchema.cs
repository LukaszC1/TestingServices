using GraphQL.Types;

public class MainSchema : Schema
{
    public MainSchema(IServiceProvider provider) : base(provider)
    {
        Query = provider.GetRequiredService<MainQuery>();
    }
}