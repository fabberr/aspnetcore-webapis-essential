using Catalog.Core.Attributes;

namespace Catalog.Core.Models.Settings;

/// <summary>
/// Represents the coniguration section for API behavior options.
/// </summary>
[ConfigurationSection(nameof(AppSettings.ApiBehavior))]
public sealed class ApiBehaviorSettings
{
    /// <summary>
    /// The maximum number of items to return for paged requests.
    /// </summary>
    public uint MaxItemsPerPage { get; init; } = default;

    /// <summary>
    /// The default number of items to return for paged requests.
    /// </summary>
    public uint DefaultItemsPerPage { get; init; } = default;
}
