namespace Temperatuurbewaking;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// Een BackgroundService die een reeks metingen langs de bewaker haalt en
// daarna de host stopt. De eigenlijke beslissing zit in Temperatuurbewaker,
// zodat de tests die klasse rechtstreeks kunnen controleren.
public sealed class BewakingsWorker : BackgroundService
{
    private static readonly double[] Metingen = [18.5, 22.0, 27.5, 31.2];

    private readonly Temperatuurbewaker _bewaker;
    private readonly ILogger<BewakingsWorker> _logger;
    private readonly IHostApplicationLifetime _lifetime;

    public BewakingsWorker(
        Temperatuurbewaker bewaker,
        ILogger<BewakingsWorker> logger,
        IHostApplicationLifetime lifetime)
    {
        _bewaker = bewaker;
        _logger = logger;
        _lifetime = lifetime;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int aantalWaarschuwingen = 0;

        foreach (double meting in Metingen)
        {
            if (_bewaker.IsTeWarm(meting))
            {
                aantalWaarschuwingen++;
            }

            await Task.Delay(TimeSpan.FromMilliseconds(300), stoppingToken);
        }

        _logger.LogInformation("Klaar: {Aantal} waarschuwing(en).", aantalWaarschuwingen);
        _lifetime.StopApplication();
    }
}
