namespace Catalog.Core.Models.Settings;

/// <summary>
/// Represents the application's settings.
/// This class cannot be inherited.
/// </summary>
public sealed class AppSettings
{
    /// <summary>
    /// The connection strings section.
    /// </summary>
    public ConnectionStringsSettings ConnectionStrings { get; init; } = new();

    /// <summary>
    /// The database provider section.
    /// </summary>
    public DatabaseProviderSettings DatabaseProvider { get; init; } = new();

    /// <summary>
    /// The API behavior section.
    /// </summary>
    public ApiBehaviorSettings ApiBehavior { get ; init ; } = new();
}
