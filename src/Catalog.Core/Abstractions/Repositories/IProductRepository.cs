using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Core.Abstractions.Repositories.Generic;
using Catalog.Core.Models.Entities;
using Catalog.Core.Repositories;

namespace Catalog.Core.Abstractions.Repositories;

/// <inheritdoc/>
public interface IProductRepository : IRepository<Product>
{
    /// <summary>
    /// Queries for the Products of a specific Category, given the Category key
    /// and  sorted by <see cref="Product.Id"/> in ascending order.
    /// </summary>
    /// <param name="categoryKey">
    /// Key that identifies a specific Category.
    /// </param>
    /// <param name="configureOptions">
    /// A delegate for configuring the options to use for this query.<br/>
    /// When not specified, uses <see cref="PaginatedQueryOptions.Default"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> to observe while waiting for the
    /// operation to complete.
    /// </param>
    /// <returns>
    /// A collection containing Products belonging to the specified Category.
    /// </returns>
    Task<IEnumerable<Product>>
    QueryMultipleByCategoryIdAsync(
        int categoryKey,
        Func<QueryOptions>? configureOptions = null,
        CancellationToken cancellationToken = default
    );
}
