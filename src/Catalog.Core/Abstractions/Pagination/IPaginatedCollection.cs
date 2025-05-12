using System.Collections.Generic;

namespace Catalog.Core.Abstractions.Pagination;

/// <summary>
/// Represents a read-only collection of a single page of elements fetched from
/// a data source.
/// </summary>
/// <typeparam name="T">
/// Type of the elements.
/// </typeparam>
public interface IPaginatedCollection<T> : IReadOnlyCollection<T>
{
    /// <summary>
    /// Gets the total number of elements in the data source.
    /// </summary>
    public int TotalCount { get; }

    /// <summary>
    /// Gets the current page number.
    /// </summary>
    public int CurrentPage { get; }

    /// <summary>
    /// Gets the number of items in each page.
    /// </summary>
    public int PageSize { get; }

    /// <summary>
    /// Gets the total number of pages.
    /// </summary>
    public int TotalPages { get; }

    /// <summary>
    /// Gets a flag that indicates whether it's possible to navigate to the next
    /// page or not.
    /// </summary>
    /// <remarks>
    /// Effectively returns <see langword="true"/> when not on the last page (
    /// <see cref="CurrentPage"/><c> &lt; </c><see cref="TotalPages"/>,
    /// <see langword="false"/> otherwise.
    /// </remarks>
    public bool HasNext { get; }

    /// <summary>
    /// Gets a flag that indicates whether it's possible to navigate the the
    /// previous page or not.
    /// </summary>
    /// <remarks>
    /// Effectively returns <see langword="true"/> when not on the first page (
    /// <see cref="CurrentPage"/><c> &gt; 1</c>), <see langword="false"/>
    /// otherwise.
    /// </remarks>
    public bool HasPrevious { get; }
}
