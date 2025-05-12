using System;
using System.Collections.Generic;
using Catalog.Core.Abstractions.Pagination;

namespace Catalog.Core.Pagination;

/// <inheritdoc cref="IPaginatedCollection{T}"/>
public sealed class PaginatedCollection<T> : List<T>, IPaginatedCollection<T>
{
    #region Backing Fields
    private readonly int _totalCount;
    private readonly int _pageNumber;
    private readonly int _pageSize;
    private readonly int _totalPages;
    #endregion

    #region Initialization
    public PaginatedCollection(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
        : base(items)
    {
        _totalCount = totalCount;
        _pageNumber = pageNumber;
        _pageSize = pageSize;
        _totalPages = (int)Math.Ceiling(_totalCount / (double)pageSize);
    }
    #endregion

    #region IPagedCollection<T>
    public int TotalCount => _totalCount;

    public int CurrentPage => _pageNumber;

    public int PageSize => _pageSize;

    public int TotalPages => _totalPages;

    public bool HasNext => CurrentPage < TotalPages;

    public bool HasPrevious => CurrentPage > 1;
    #endregion
}
