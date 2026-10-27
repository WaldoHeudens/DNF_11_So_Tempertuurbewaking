namespace Temperatuurbewaking.Tests;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

public class TemperatuurbewakerTests
{
    // Bouwt een bewaker met een vaste grens. Options.Create maakt een IOptions<T>
    // zonder configuratie of container; handig om de logica los te testen.
    private static Temperatuurbewaker MaakBewaker(double grens, ILogger<Temperatuurbewaker>? logger = null)
    {
        IOptions<MeldingOptions> opties = Options.Create(new MeldingOptions
        {
            Locatie = "Test",
            MaxTemperatuur = grens,
        });

        return new Temperatuurbewaker(opties, logger ?? NullLogger<Temperatuurbewaker>.Instance);
    }

    [Theory]
    [InlineData(26, true)]
    [InlineData(25, false)] // gelijk aan de grens is niet te warm
    [InlineData(10, false)]
    public void IsTeWarm_vergelijkt_met_de_grens(double temperatuur, bool verwacht)
    {
        Temperatuurbewaker bewaker = MaakBewaker(grens: 25);

        Assert.Equal(verwacht, bewaker.IsTeWarm(temperatuur));
    }

    [Fact]
    public void Te_warme_meting_logt_een_waarschuwing()
    {
        TestLogger<Temperatuurbewaker> logger = new();
        Temperatuurbewaker bewaker = MaakBewaker(grens: 25, logger);

        bewaker.IsTeWarm(30);

        Assert.Contains(logger.Regels, regel => regel.Level == LogLevel.Warning);
    }

    [Fact]
    public void Normale_meting_logt_geen_waarschuwing()
    {
        TestLogger<Temperatuurbewaker> logger = new();
        Temperatuurbewaker bewaker = MaakBewaker(grens: 25, logger);

        bewaker.IsTeWarm(20);

        Assert.DoesNotContain(logger.Regels, regel => regel.Level == LogLevel.Warning);
    }
}
