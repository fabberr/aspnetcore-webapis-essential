using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Core.Abstractions.Repositories.Generic.Interfaces;
using Catalog.Core.Models.Entities;

namespace Catalog.Core.Abstractions.Repositories.Interfaces;

/// <inheritdoc/>
public interface ICategoryRepository : IRepository<Category, int>
{
    /// <summary>
    /// Queries for the Products of a specific Category, given its key.
    /// </summary>
    /// <param name="key">
    /// The Category's key.
    /// </param>
    /// <param name="limit">
    /// Delimits the number of entries which will be fetched at most.
    /// </param>
    /// <param name="offset">
    /// Number of entries to skip.
    /// </param>
    /// <returns>
    /// An enumerable collection containing Products belonging to the specified
    /// Category, or <see langword="null"/> if no Products were found.
    /// </returns>
    Task<IEnumerable<Product>?> GetProducts(int key, uint limit = 10u, uint offset = 0u);
}
