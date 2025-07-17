using RustRetail.APIGateway.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080);
    options.ListenAnyIP(8081, listenOption =>
    {
        listenOption.UseHttps(
            builder.Configuration.GetValue<string>("HttpsCert:FileName")!,
            builder.Configuration.GetValue<string>("HttpsCert:Password")!);
    });
});

var app = builder.Build();

app.ConfigureApplicationPipeline();

app.Run();
