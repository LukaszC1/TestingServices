using GraphQL;
using GraphQL.Execution;
using GraphQL.Server.Ui.GraphiQL;
using GraphQL.Types;
using GraphqlService.Mutations;
using GraphqlService.Queries;
using GraphqlService.Schemas;
using GraphqlService.Types;
using LocalRepository;

namespace GraphqlService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSingleton<ProductType>();
            builder.Services.AddSingleton<CustomerType>();
            builder.Services.AddSingleton<EmployeeType>();
            builder.Services.AddSingleton<OrderType>();
            builder.Services.AddSingleton<OrderDetailType>();
            builder.Services.AddSingleton<MainQuery>();
            builder.Services.AddSingleton<OrderWithDetailsType>();
            builder.Services.AddSingleton<CustomerWithOrdersType>();
            builder.Services.AddSingleton<OrderInputType>();
            builder.Services.AddSingleton<CustomerInputType>();
            builder.Services.AddSingleton<MainMutation>();

            builder.Services.AddSingleton<ISchema, MainSchema>();
            builder.Services.AddSingleton<IRepository, LocalRepository.LocalRepository>();
            builder.Services.AddSingleton<IErrorInfoProvider>(sp =>
                new ErrorInfoProvider(new ErrorInfoProviderOptions
                {
                    ExposeExceptionDetails = true
                }));

            builder.Services.AddGraphQL(b => b
                .AddSchema<MainSchema>()
                .AddSystemTextJson());
                
            var app = builder.Build();

            app.UseRouting();
            app.MapGraphQL("/graphql");
            app.UseGraphQLGraphiQL(
                "/",
                new GraphiQLOptions
                {
                    GraphQLEndPoint = "/graphql",
                    SubscriptionsEndPoint = "/graphql",
                });

            app.Run();
        }
    }
}
