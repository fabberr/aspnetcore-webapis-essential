using Catalog.Core.Models.Entities;
using Catalog.Core.Models.Parameters;

namespace Catalog.Core.Repositories;

/// <summary>
/// Represents a set of options for querying paginated entities from the data
/// source.
/// </summary>
/// <remarks>
/// Specialization of <see cref="QueryOptions"/>.
/// </remarks>
/// <param name="Pagination">
/// Pagination options.
/// </param>
/// <param name="IncludeHiddenEntities">
/// Whether hidden entities (<see cref="EntityBase.Hidden"/> is set to
/// <see langword="true"/>) entities should be included in the query or not.
/// </param>
/// <param name="TrackChanges">
/// Whether to track changes made to the entities returned from the query or not.
/// </param>
public sealed record PaginatedQueryOptions(
    PaginationParameters Pagination,
    bool IncludeHiddenEntities = false,
    bool TrackChanges = false
) : QueryOptions(
    IncludeHiddenEntities: IncludeHiddenEntities,
    TrackChanges: TrackChanges
)
{
    private readonly static PaginatedQueryOptions _defaultPaginatedQueryOptions = new(
        Pagination: PaginationParameters.Default,
        IncludeHiddenEntities: _defaultQueryOptions.IncludeHiddenEntities,
        TrackChanges: _defaultQueryOptions.TrackChanges
    );

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
    ///         <term><c><see cref="QueryOptions.IncludeHiddenEntities"/></c></term>
    ///         <description><see langword="false"/></description>
    ///     </item>
    ///     <item>
    ///         <term><c><see cref="QueryOptions.TrackChanges"/></c></term>
    ///         <description><see langword="false"/></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public static new PaginatedQueryOptions Default => _defaultPaginatedQueryOptions;
}
