namespace Catalog.Core.Models.Settings;

/// <summary>
/// Represents the configuration section for the EF Core databse provider options.
/// </summary>
public class DatabaseProviderSettings
{
    /// <summary>
    /// The default provider.
    /// </summary>
    public DatabaseProvider Default { get; init; } = default;
}

/// <summary>
/// Enumerates the supported EF Core database providers.
/// </summary>
public enum DatabaseProvider
{
    /// <summary>
    /// <see href="https://www.sqlite.org">SQLite</see> database engine.
    /// </summary>
    /// <remarks>
    /// Uses the <see href="https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite">
    /// Microsoft.EntityFrameworkCore.Sqlite</see> package, which only supports SQLite version 3.7 onwards.
    /// </remarks>
    Sqlite3 = default,
}
