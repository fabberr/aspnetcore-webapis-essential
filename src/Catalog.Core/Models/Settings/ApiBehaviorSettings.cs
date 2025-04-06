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

    public ApiDeleteBehavior DeleteBehavior { get; init; } = default;
}

/// <summary>
/// Enumerates the supported approaches for deleting entitites on the data store
/// through an API call.
/// </summary>
public enum ApiDeleteBehavior
{
    /// <summary>
    /// Physically deletes the data from the data store.
    /// </summary>
    Physical = default,

    /// <summary>
    /// Mark the entry as hidden such that it's treated as deleted on all
    /// subsequent queries from the data store.<br/>
    /// Once marked as hidden, an entity cannot be unhidden via the API.
    /// </summary>
    Logical,
}
