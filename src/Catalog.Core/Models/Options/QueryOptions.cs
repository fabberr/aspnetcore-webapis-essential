using Catalog.Core.Models.Entities;

namespace Catalog.Core.Models.Options;

/// <summary>
/// Represents a set of options for querying entities from the data source.
/// </summary>
/// <remarks>
/// This type serves as a base for all other query option types.
/// </remarks>
/// <param name="IncludeHiddenEntities">
/// Whether hidden entities (<see cref="EntityBase.Hidden"/> is set to
/// <see langword="true"/>) entities should be included in the query or not.
/// </param>
/// <param name="TrackChanges">
/// Whether to track changes made to the entities returned from the query or not.
/// </param>
public record QueryOptions(
    bool IncludeHiddenEntities = false,
    bool TrackChanges = false
)
{
    /// <summary>
    /// Represents a set of default query options.
    /// </summary>
    /// <remarks>
    /// The options will be set as follows:
    /// <list type="bullet">
    ///     <item>
    ///         <term><c><see cref="IncludeHiddenEntities"/></c></term>
    ///         <description><c><see langword="false"/></c></description>
    ///     </item>
    ///     <item>
    ///         <term><c><see cref="TrackChanges"/></c></term>
    ///         <description><c><see langword="false"/></c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public static QueryOptions Default => new(
        IncludeHiddenEntities: false,
        TrackChanges: false
    );
}
