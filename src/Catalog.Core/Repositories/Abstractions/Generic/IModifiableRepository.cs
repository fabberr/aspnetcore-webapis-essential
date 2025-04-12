using System.Threading;
using System.Threading.Tasks;
using Catalog.Core.Enums;
using Catalog.Core.Models.Entities;

namespace Catalog.Core.Repositories.Abstractions.Generic;

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
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> for cancelling the operation.
    /// </param>
    /// <returns>
    /// The created entity.
    /// </returns>
    Task<TEntity> CreateAsync(
        TEntity entity,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Updates an existing entity of type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <param name="entity">
    /// The entity to update.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> for cancelling the operation.
    /// </param>
    /// <returns>
    /// The updated entity.
    /// </returns>
    Task<TEntity> UpdateAsync(
        TEntity entity,
        CancellationToken cancellationToken = default
    );

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
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> for cancelling the operation.
    /// </param>
    /// <returns>
    /// The deleted entity, or <see langword="null"/> if no entity with the
    /// given <paramref name="key"/> was found.
    /// </returns>
    Task<TEntity?> DeleteByIdAsync(
        int key,
        DeleteStrategy strategy = DeleteStrategy.Delete,
        CancellationToken cancellationToken = default
    );
}
