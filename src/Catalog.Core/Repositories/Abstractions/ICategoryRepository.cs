using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Core.Models.Entities;
using Catalog.Core.Repositories.Abstractions.Generic;

namespace Catalog.Core.Repositories.Abstractions;

/// <inheritdoc/>
public interface ICategoryRepository : IRepository<Category>
{
    /// <summary>
    /// Queries for the Products of a specific Category, given its key and
    /// sorted by <see cref="Product.Id"/> in ascending order.
    /// </summary>
    /// <remarks>
    /// Note: Products marked as hidden (<see cref="Product.Hidden"/> is set to
    /// <see langword="true"/>) will <b>not</b> be fetched from the data source
    /// when using this method.
    /// </remarks>
    /// <param name="categoryKey">
    /// Key that identifies a specific Category.
    /// </param>
    /// <param name="limit">
    /// Delimits the number of entries which will be fetched at most.
    /// </param>
    /// <param name="offset">
    /// Number of entries to skip.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> for cancelling the operation.
    /// </param>
    /// <returns>
    /// An enumerable collection containing Products belonging to the specified
    /// Category.
    /// </returns>
    Task<IEnumerable<Product>>
    GetProducts(
        int categoryKey,
        uint limit = 10u,
        uint offset = 0u,
        CancellationToken cancellationToken = default
    );
}
