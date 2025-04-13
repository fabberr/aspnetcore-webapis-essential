using Catalog.Core.Models.Entities;

namespace Catalog.Core.Models.Options;

/// <summary>
/// Represents a set of options for querying entities with pagination from the
/// data source.
/// </summary>
/// <remarks>
/// Specialization of <see cref="QueryOptions"/>.
/// </remarks>
/// <param name="IncludeHiddenEntities">
/// Whether hidden entities (<see cref="EntityBase.Hidden"/> is set to
/// <see langword="true"/>) entities should be included in the query or not.
/// </param>
/// <param name="Limit">
/// Delimits the number of entries which will be fetched at most.<br/>
/// Set to <c>-1</c> to disable pagination for this query.
/// </param>
/// <param name="Offset">
/// Number of entries to skip.
/// </param>
public record PaginatedQueryOptions(bool IncludeHiddenEntities = false, int Limit = 10, int Offset = 0)
    : QueryOptions(IncludeHiddenEntities)
{
    /// <summary>
    /// Represents a set of default query options.
    /// </summary>
    /// <remarks>
    /// The options will be set as follows:
    /// <list type="bullet">
    ///     <item>
    ///         <term><c><see cref="QueryOptions.IncludeHiddenEntities"/></c></term>
    ///         <description><c><see langword="false"/></c></description>
    ///     </item>
    ///     <item>
    ///         <term><c><see cref="Limit"/></c></term>
    ///         <description><c>10</c></description>
    ///     </item>
    ///     <item>
    ///         <term><c><see cref="Offset"/></c></term>
    ///         <description><c>0</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public static new PaginatedQueryOptions Default => new(
        IncludeHiddenEntities: QueryOptions.Default.IncludeHiddenEntities,
        Limit: 10,
        Offset: 0
    );
}
