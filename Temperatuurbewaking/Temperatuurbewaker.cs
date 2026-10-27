namespace Temperatuurbewaking;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

// De bewaker krijgt zijn grens via IOptions<MeldingOptions> en logt via ILogger<T>.
// Beide komen via de constructor binnen, zodat je de klasse los kan testen.
public sealed class Temperatuurbewaker
{
    private readonly MeldingOptions _opties;
    private readonly ILogger<Temperatuurbewaker> _logger;

    public Temperatuurbewaker(IOptions<MeldingOptions> opties, ILogger<Temperatuurbewaker> logger)
    {
        _opties = opties.Value;
        _logger = logger;
    }

    // Geeft true als de temperatuur strikt boven de ingestelde grens ligt.
    public bool IsTeWarm(double temperatuur)
    {
        if (temperatuur > _opties.MaxTemperatuur)
        {
            _logger.LogWarning(
                "{Locatie}: {Temperatuur} graden ligt boven de grens van {Max}",
                _opties.Locatie, temperatuur, _opties.MaxTemperatuur);
            return true;
        }

        _logger.LogInformation(
            "{Locatie}: {Temperatuur} graden is binnen de grens",
            _opties.Locatie, temperatuur);
        return false;
    }
}
