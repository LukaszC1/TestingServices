using LocalRepository;

var builder = WebApplication.CreateBuilder();

builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();
builder.Services.AddSingleton<IRepository, LocalRepository.LocalRepository>();
builder.Services.AddSingleton<SoapService>();

builder.Services.AddSingleton<IServiceBehavior>(new ServiceBehaviorAttribute
{
    IncludeExceptionDetailInFaults = true
});

builder.Services.TryAddEnumerable(
    ServiceDescriptor.Singleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>());

var app = builder.Build();

app.UseServiceModel(serviceBuilder =>
{
    serviceBuilder.AddService<SoapService>()
                  .AddServiceEndpoint<SoapService, ISoapService>(new BasicHttpBinding(), "/SoapService.svc");
    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    serviceMetadataBehavior.HttpGetEnabled = true;
});
    
app.Run();
