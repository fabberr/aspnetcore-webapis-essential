using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Core.Abstractions.Repositories.Generic.Interfaces;
using Catalog.Core.Models.Entities;

namespace Catalog.Core.Repositories.Interfaces;

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
    /// <returns>
    /// An enumerable collection containing Products belonging to the specified
    /// Category.
    /// </returns>
    Task<IEnumerable<Product>> GetProducts(int categoryKey, uint limit = 10u, uint offset = 0u);
}
