using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Catalog.Api.Logging;

/// <summary>
///     Implements <see cref="ILogger"/> for logging to an append-only file on
///     the local disk.
/// </summary>
/// <param name="categoryName">
///     The category name for messages produced by the logger.
/// </param>
/// <param name="loggerConfiguration">
///     The configuration for the logger.
/// </param>
public sealed class AppendOnlyFileLogger(string categoryName, AppendOnlyFileLoggerProviderConfiguration loggerConfiguration)
    : ILogger
{
    #region Constants
    private readonly static FileStreamOptions _stAppendOnlyFileStreamOptions = new() {
        Access = FileAccess.Write,
        Mode = FileMode.OpenOrCreate,
        // Share = FileShare.ReadWrite,
        // Options = FileOptions.WriteThrough | FileOptions.SequentialScan | FileOptions.Asynchronous,
        // BufferSize = 0,
    };
    #endregion

    #region Fields
    private readonly string _categoryName = categoryName;
    private readonly AppendOnlyFileLoggerProviderConfiguration _loggerConfiguration = loggerConfiguration;
    #endregion

    #region ILogger, ILogger<out TCategoryName>
    /// <inheritdoc/>
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public bool IsEnabled(LogLevel logLevel) => logLevel == _loggerConfiguration.LogLevel;

    /// <inheritdoc/>
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        EnsureValidRootPath();

        var message = $"[{DateTime.UtcNow:dd/MM/yyyy hh:mm:ss.fffffff}] {logLevel}: {_categoryName}[{eventId.Id}] - {formatter(state, exception)}"
            .AsSpan();

        using var file = new StreamWriter(
            path: _loggerConfiguration.Filename,
            encoding: Encoding.UTF8,
            append: true
        );

        file.WriteLine(message);
    }
    #endregion

    #region Private
    private void EnsureValidRootPath()
    {
        string rootPath = Path.GetDirectoryName(Path.GetFullPath(_loggerConfiguration.Filename))!;
        var rootDirectory = new DirectoryInfo(rootPath);

        if (rootDirectory.Exists)
        {
            return;
        }

        rootDirectory.Create();
    }
    #endregion
}
