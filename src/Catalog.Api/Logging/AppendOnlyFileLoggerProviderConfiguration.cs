using System;
using Microsoft.Extensions.Logging;

namespace Catalog.Api.Logging;

public sealed class AppendOnlyFileLoggerProviderConfiguration
{
    /// <summary>
    ///     The severity level.
    /// </summary>
    public LogLevel LogLevel { get; set; } = LogLevel.Warning;

    /// <summary>
    ///     The Id of the logging event.
    /// </summary>
    public int EventId { get; set; } = default;

    /// <summary>
    ///     Path to the append-only file on disk.
    /// </summary>
    public string Filename { get; set; } = "./app.log";
}
