namespace Temperatuurbewaking.Tests;

using Microsoft.Extensions.Logging;

// Een minimale ILogger die elke log-regel bijhoudt, zodat een test kan
// controleren wat er gelogd werd zonder een echte console of host nodig te hebben.
public sealed class TestLogger<T> : ILogger<T>
{
    public List<(LogLevel Level, string Bericht)> Regels { get; } = [];

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        Regels.Add((logLevel, formatter(state, exception)));
    }
}
