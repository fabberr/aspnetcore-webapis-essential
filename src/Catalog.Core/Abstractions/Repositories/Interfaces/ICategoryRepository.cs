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
    /// <returns>
    /// An enumerable collection containing all Products belonging to the
    /// specified Category, or <see langword="null"/> if no Products were found.
    /// </returns>
    Task<IEnumerable<Product>?> GetProducts(int key);
}
