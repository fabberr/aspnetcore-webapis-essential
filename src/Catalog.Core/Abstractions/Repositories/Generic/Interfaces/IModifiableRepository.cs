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
    /// The entity to create.
    /// </param>
    /// <returns>
    /// The created entity.
    /// </returns>
    Task<TEntity> CreateAsync(TEntity entity);

    /// <summary>
    /// Updates an existing entity of type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <param name="entity">
    /// The entity to update.
    /// </param>
    /// <returns>
    /// The updated entity.
    /// </returns>
    Task<TEntity> UpdateAsync(TEntity entity);

    /// <summary>
    /// Deletes an existing entity of type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <param name="entity">
    /// The entity to delete.
    /// </param>
    /// <returns>
    /// The deleted entity.
    /// </returns>
    Task<TEntity> DeleteAsync(TEntity entity);
}
