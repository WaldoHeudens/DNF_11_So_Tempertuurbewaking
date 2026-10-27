namespace Temperatuurbewaking;

// De sectie "Melding" uit appsettings.json wordt naar deze klasse gebonden.
public sealed class MeldingOptions
{
    public string Locatie { get; set; } = "onbekend";

    public double MaxTemperatuur { get; set; }
}
