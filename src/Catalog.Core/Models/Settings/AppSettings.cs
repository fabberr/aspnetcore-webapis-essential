namespace Catalog.Core.Models.Settings;

/// <inheritdoc cref="IAppSettings" />
public class AppSettings : IAppSettings
{
    /// <inheritdoc />
    public ConnectionStringsSettings ConnectionStrings { get; init; } = new();

    /// <inheritdoc />
    public DatabaseProviderSettings DatabaseProvider { get; init; } = new();
}
