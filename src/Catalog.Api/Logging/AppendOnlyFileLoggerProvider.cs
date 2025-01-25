using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Catalog.Api.Logging;

public sealed class AppendOnlyFileLoggerProvider(AppendOnlyFileLoggerProviderConfiguration loggerConfiguration)
    : ILoggerProvider
{
    #region Fields
    private readonly AppendOnlyFileLoggerProviderConfiguration _loggerConfiguration = loggerConfiguration;
    private readonly ConcurrentDictionary<string, AppendOnlyFileLogger> _instances = new();
    #endregion

    #region ILoggerProvider
    /// <inheritdoc/>
    public ILogger CreateLogger(string categoryName) => _instances.GetOrAdd(
        key: categoryName,
        value: new AppendOnlyFileLogger(categoryName, _loggerConfiguration)
    );

    /// <inheritdoc/>
    public void Dispose()
    {
        _instances.Clear();
    }
    #endregion
}
