namespace Catalog.Api.Models.Settings;

public class AppSettings
{
    public DatabaseProvider DefaultDatabaseProvider { get; init; } = default;
}
