using Catalog.Core.Models.Entities;
using Catalog.Core.Models.Parameters;

namespace Catalog.Core.Repositories;

/// <summary>
/// Represents a set of options for querying entities from the data source.
/// </summary>
/// <remarks>
/// This type serves as a base for all other query option types.
/// </remarks>
/// <param name="Pagination">
/// Pagination options.<br/>
/// <br/>
/// <b>Warning:</b> When not specified, no pagination logic will be applied to
/// the query and <i>all entries will be fetched from the data source</i>. Use
/// with caution.
/// </param>
/// <param name="IncludeHiddenEntities">
/// Whether hidden entities (<see cref="EntityBase.Hidden"/> is set to
/// <see langword="true"/>) entities should be included in the query or not.
/// </param>
/// <param name="TrackChanges">
/// Whether to track changes made to the entities returned from the query or not.
/// </param>
public sealed record QueryOptions(
    PaginationParameters? Pagination,
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
    ///         <term><c><see cref="Pagination"/></c></term>
    ///         <description><see cref="PaginationParameters.Default"/></description>
    ///     </item>
    ///     <item>
    ///         <term><c><see cref="IncludeHiddenEntities"/></c></term>
    ///         <description><see langword="false"/></description>
    ///     </item>
    ///     <item>
    ///         <term><c><see cref="TrackChanges"/></c></term>
    ///         <description><see langword="false"/></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public static QueryOptions Default => new(
        Pagination: PaginationParameters.Default,
        IncludeHiddenEntities: false,
        TrackChanges: false
    );
}
