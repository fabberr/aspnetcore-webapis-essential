using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Core.Enums;
using Catalog.Core.Models.Entities;

namespace Catalog.Core.Abstractions.Repositories.Generic;

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
    /// A <see cref="CancellationToken"/> to observe while waiting for the
    /// operation to complete.
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
    /// A <see cref="CancellationToken"/> to observe while waiting for the
    /// operation to complete.
    /// </param>
    /// <returns>
    /// The updated entity.
    /// </returns>
    Task<TEntity> UpdateAsync(
        TEntity entity,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Removes an existing entity of type <typeparamref name="TEntity"/> by its
    /// key.
    /// </summary>
    /// <param name="key">
    /// Key that identifies a specific entity.
    /// </param>
    /// <param name="strategy">
    /// The strategy to apply when removing the entity.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> to observe while waiting for the
    /// operation to complete.
    /// </param>
    /// <returns>
    /// The removed entity, or <see langword="null"/> if no entity with the
    /// given <paramref name="key"/> was found.
    /// </returns>
    Task<TEntity?> RemoveByIdAsync(
        int key,
        RemoveStrategy strategy = RemoveStrategy.Delete,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Removes existing entities of type <typeparamref name="TEntity"/> that
    /// meet the search criteria defined in <paramref name="predicate"/>.
    /// </summary>
    /// <param name="predicate">
    /// Repesents the search criteria as a predicate expression.
    /// </param>
    /// <param name="strategy">
    /// The strategy to apply when removing the entities.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> to observe while waiting for the
    /// operation to complete.
    /// </param>
    /// <returns>
    /// A collection containing entities of type <typeparamref name="TEntity"/>
    /// that meet the search criteria and were removed successfully.
    /// </returns>
    Task<IEnumerable<TEntity>> RemoveByPredicateAsync(
        Expression<Func<TEntity, bool>> predicate,
        RemoveStrategy strategy = RemoveStrategy.Delete,
        CancellationToken cancellationToken = default
    );
}
