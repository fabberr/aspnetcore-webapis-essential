using System.Threading.Tasks;
using Catalog.Core.Models.Entities;

namespace Catalog.Core.Abstractions.Repositories.Generic.Interfaces;

/// <summary>
/// A repository capable of modifying data from entities of type
/// <typeparamref name="TEntity"/>.
/// </summary>
/// <typeparam name="TEntity">
/// Type of the entity.
/// </typeparam>
/// <typeparam name="TKey">
/// Type of the key used to identify the entity.
/// </typeparam>
public interface IModifiableRepository<TEntity, TKey>
    where TEntity : EntityBase
    where TKey : notnull
{
    /// <summary>
    /// Creates a new entity of type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <param name="entity">
    /// Data to create the new entity with.
    /// </param>
    /// <returns>
    /// The created entity, or <see langword="null"/> if the operation failed.
    /// </returns>
    Task<TEntity?> CreateAsync(TEntity entity);

    /// <summary>
    /// Updates an existing entity of type <typeparamref name="TEntity"/>, given
    /// its key.
    /// </summary>
    /// <param name="key">
    /// The entity's key.
    /// </param>
    /// <param name="entity">
    /// Data to modify the existing entity with.
    /// </param>
    /// <returns>
    /// The modified entity, or <see langword="null"/> if the operation failed.
    /// </returns>
    Task<TEntity?> UpdateAsync(TKey key, TEntity entity);

    /// <summary>
    /// Deletes an existing entity of type <typeparamref name="TEntity"/>, given
    /// its key.
    /// </summary>
    /// <param name="key">
    /// The entity's key.
    /// </param>
    /// <returns>
    /// The deleted entity, or <see langword="null"/> if the operation failed.
    /// </returns>
    Task<TEntity?> DeleteAsync(TKey key);
}
