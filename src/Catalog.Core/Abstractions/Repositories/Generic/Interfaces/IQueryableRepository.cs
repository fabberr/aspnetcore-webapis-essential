using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Core.Models.Entities;

namespace Catalog.Core.Abstractions.Repositories.Generic.Interfaces;

/// <summary>
/// A repository capable of querying data from entities of type
/// <typeparamref name="TEntity"/>.
/// </summary>
/// <typeparam name="TEntity">
/// Type of the entity.
/// </typeparam>
/// <typeparam name="TKey">
/// Type of the key used to identify a specific entity of type
/// <typeparamref name="TEntity"/>.
/// </typeparam>
public interface IQueryableRepository<TEntity, TKey>
    where TEntity : EntityBase
    where TKey : notnull
{
    /// <summary>
    /// Queries for entities of type <typeparamref name="TEntity"/>, in
    /// ascending <see cref="EntityBase.Id"/> order.
    /// </summary>
    /// <param name="limit">
    /// Delimits the number of entries which will be fetched at most.
    /// </param>
    /// <param name="offset">
    /// Number of entries to skip.
    /// </param>
    /// <returns>
    /// An enumerable collection containing entities of type
    /// <typeparamref name="TEntity"/>.
    /// </returns>
    Task<IEnumerable<TEntity>> GetAsync(uint limit = 10u, uint offset = 0u);

    /// <summary>
    /// Queries for a specific entity of type <typeparamref name="TEntity"/>,
    /// given its key.
    /// </summary>
    /// <param name="key">
    /// Key that uniquely identifies the entity.
    /// </param>
    /// <returns>
    /// The entity identified by <paramref name="key"/>, or <see langword="null"/>
    /// if no entity was found.
    /// </returns>
    Task<TEntity?> GetAsync(TKey key);
}
