using Gateway.App.DeleteHandlers;
using Ocelot.DependencyInjection;
using Ocelot.Middleware; 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOcelot();
var services = builder.Services;
services.AddHttpClient<TokenExhangeDelegateHandler>();
services.AddAuthentication().AddJwtBearer("GatewayAuthenticationScheme", options =>
{
    options.Authority = builder.Configuration["IdentityServerURL"];
    options.Audience = "resource_gateway";
    options.RequireHttpsMetadata = false;
});

services.AddOcelot().AddDelegatingHandler<TokenExhangeDelegateHandler>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

await app.UseOcelot();
Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile($"configuration.{hostingContext.HostingEnvironment.EnvironmentName.ToLower()}.json").AddEnvironmentVariables();

});
app.Run();
