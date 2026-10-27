namespace Temperatuurbewaking.Tests;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class ConfiguratieTests
{
    [Fact]
    public void Configuratie_bindt_naar_MeldingOptions()
    {
        Dictionary<string, string?> instellingen = new()
        {
            ["Melding:Locatie"] = "Kelder",
            ["Melding:MaxTemperatuur"] = "18.5",
        };

        IConfiguration configuratie = new ConfigurationBuilder()
            .AddInMemoryCollection(instellingen)
            .Build();

        MeldingOptions opties = configuratie.GetSection("Melding").Get<MeldingOptions>()!;

        Assert.Equal("Kelder", opties.Locatie);
        Assert.Equal(18.5, opties.MaxTemperatuur);
    }

    [Fact]
    public void Container_injecteert_gebonden_options_in_de_bewaker()
    {
        Dictionary<string, string?> instellingen = new()
        {
            ["Melding:Locatie"] = "Serverruimte",
            ["Melding:MaxTemperatuur"] = "25",
        };

        IConfiguration configuratie = new ConfigurationBuilder()
            .AddInMemoryCollection(instellingen)
            .Build();

        ServiceCollection services = new();
        services.AddLogging();
        services.Configure<MeldingOptions>(configuratie.GetSection("Melding"));
        services.AddSingleton<Temperatuurbewaker>();

        using ServiceProvider provider = services.BuildServiceProvider();
        Temperatuurbewaker bewaker = provider.GetRequiredService<Temperatuurbewaker>();

        Assert.True(bewaker.IsTeWarm(30));  // boven de grens van 25
        Assert.False(bewaker.IsTeWarm(20)); // onder de grens van 25
    }
}
