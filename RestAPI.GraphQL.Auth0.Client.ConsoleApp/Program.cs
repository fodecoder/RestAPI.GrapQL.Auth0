using Common.Services.Extension;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestAPI.GraphQL.Auth0.Client.ConsoleApp;
using RestAPI.GraphQL.Auth0.Server.BL.Interfaces.ProxyServices;
using RestAPI.GraphQL.Auth0.Server.BL.Interfaces.Service;

HostApplicationBuilder builder = Host.CreateApplicationBuilder ( args );
builder.Configuration.Sources.Clear ();

IHostEnvironment env = builder.Environment;

builder.Configuration
    .AddJsonFile ( "appsettings.json" , optional: true , reloadOnChange: true )
    .AddJsonFile ( $"appsettings.{env.EnvironmentName}.json" , true , true );

builder.Services.AddProxy<IItemService , ProxyItemService> ( builder.Configuration );
builder.Services.AddTransient<Tester> ();

using IHost host = builder.Build ();

await ExemplifyServices ( host.Services );

static async Task ExemplifyServices( IServiceProvider hostProvider )
{
    using IServiceScope serviceScope = hostProvider.CreateScope ();
    IServiceProvider provider = serviceScope.ServiceProvider;
    Tester tester = provider.GetRequiredService<Tester> ();
    await tester.Run ();

    Console.WriteLine ();
}