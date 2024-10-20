namespace Catalog.Core.Models.Settings;

/// <summary>
/// Represents the configuration section for storing connection strings to 
/// various connected services.
/// </summary>
public class ConnectionStringsSettings
{
    /// <summary>
    /// The connection string for the main SQL database.
    /// </summary>
    public string SqlDatabase { get; init; } = string.Empty;
}
