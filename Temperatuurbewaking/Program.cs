using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Temperatuurbewaking;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

// Configuratie, DI en logging samen: bind de options, registreer de bewaker
// als service en de worker als achtergronddienst.
builder.Services.Configure<MeldingOptions>(builder.Configuration.GetSection("Melding"));
builder.Services.AddSingleton<Temperatuurbewaker>();
builder.Services.AddHostedService<BewakingsWorker>();

builder.Build().Run();
