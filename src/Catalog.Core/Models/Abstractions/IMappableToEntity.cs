using Catalog.Core.Models.Entities;

namespace Catalog.Core.Models.Abstractions;

/// <summary>
/// Defines a method for creating <typeparamref name="TEntity"/> instances from
/// other types.
/// </summary>
/// <typeparam name="TEntity">
/// Type of the entity.
/// </typeparam>
public interface IMappableToEntity<TEntity>
    where TEntity : EntityBase
{
    /// <summary>
    /// Creates an instance of <typeparamref name="TEntity"/>.
    /// </summary>
    /// <returns>
    /// The <typeparamref name="TEntity"/> entity.
    /// </returns>
    TEntity ToEntity();
}
