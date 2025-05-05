using Catalog.Core.Attributes;
using Catalog.Core.Enums;

namespace Catalog.Core.Models.Settings;

/// <summary>
/// Represents the configuration section for API behavior options.
/// </summary>
[ConfigurationSection(nameof(AppSettings.ApiBehavior))]
public sealed class ApiBehaviorSettings
{
    /// <summary>
    /// The maximum number of items to return for paginated requests.
    /// </summary>
    public uint MaxPageSize { get; init; } = default;

    /// <summary>
    /// The default number of items to return for paginated requests.
    /// </summary>
    public uint DefaultPageSize { get; init; } = default;

    /// <summary>
    /// Which strategy to use when removing entitites through API requests.
    /// </summary>
    public RemoveStrategy RemoveStrategy { get; init; } = default;
}
