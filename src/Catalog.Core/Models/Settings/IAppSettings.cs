namespace Catalog.Core.Models.Settings;

/// <summary>
/// Represents the application's settings.
/// </summary>
public interface IAppSettings
{
    /// <summary>
    /// The connection strings section.
    /// </summary>
    ConnectionStringsSettings ConnectionStrings { get; init; }

    /// <summary>
    /// The database provider section.
    /// </summary>
    DatabaseProviderSettings DatabaseProvider { get; init; }
}
