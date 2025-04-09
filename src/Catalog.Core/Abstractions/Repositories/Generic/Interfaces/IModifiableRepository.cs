using System.Threading.Tasks;
using Catalog.Core.Enums;
using Catalog.Core.Models.Entities;

namespace Catalog.Core.Abstractions.Repositories.Generic.Interfaces;

/// <summary>
/// A repository capable of modifying data from entities of type
/// <typeparamref name="TEntity"/>.
/// </summary>
/// <typeparam name="TEntity">
/// Type of the entity.
/// </typeparam>
public interface IModifiableRepository<TEntity>
    where TEntity : EntityBase
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
    /// Deletes an existing entity of type <typeparamref name="TEntity"/> by its
    /// key.
    /// </summary>
    /// <param name="key">
    /// Key that identifies a specific entity.
    /// </param>
    /// <param name="strategy">
    /// The strategy to apply when deleting the entity.
    /// </param>
    /// <returns>
    /// The deleted entity, or <see langword="null"/> if no entity with the
    /// given <paramref name="key"/> was found.
    /// </returns>
    Task<TEntity?> DeleteAsync(int key, DeleteStrategy strategy = DeleteStrategy.Delete);
}
