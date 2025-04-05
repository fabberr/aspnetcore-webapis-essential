using System.Collections.Generic;
using System.Linq;
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
    /// Exposes a <see cref="IQueryable{T}"/> object for querying entities of
    /// type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <returns>
    /// A <see cref="IQueryable{T}"/> object for querying entities of type
    /// <typeparamref name="TEntity"/>.
    /// </returns>
    Task<IQueryable<TEntity>> QueryAsync();

    /// <summary>
    /// Queries for entities of type <typeparamref name="TEntity"/>, in
    /// ascending <see cref="EntityBase.Id"/> order.
    /// </summary>
    /// <param name="limit">
    /// Delimits the number of entities which will be fetched at most.
    /// </param>
    /// <param name="offset">
    /// Number of entities to skip.
    /// </param>
    /// <param name="includeRelated">
    /// Whether to include related entities in the query or not.
    /// </param>
    /// <returns>
    /// An enumerable collection containing entities of type
    /// <typeparamref name="TEntity"/>, or <see langword="null"/> if no entities
    /// were found.
    /// </returns>
    Task<IEnumerable<TEntity>?> GetAsync(uint limit = 10u, uint offset = 0u, bool includeRelated = false);

    /// <summary>
    /// Queries for a specific entity of type <typeparamref name="TEntity"/>,
    /// given its key.
    /// </summary>
    /// <param name="key">
    /// The entity's key.
    /// </param>
    /// <param name="includeRelated">
    /// Whether to include related entities in the query or not.
    /// </param>
    /// <returns>
    /// The entity of type <typeparamref name="TEntity"/> 
    /// </returns>
    Task<TEntity?> GetAsync(TKey key, bool includeRelated = false);
}
