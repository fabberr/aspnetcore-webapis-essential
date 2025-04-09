using Catalog.Core.Models.Entities;

namespace Catalog.Core.Abstractions.Repositories.Generic.Interfaces;

/// <summary>
/// A repository capbale of both querying and modifying data from entities of
/// type <typeparamref name="TEntity"/>.
/// </summary>
/// <typeparam name="TEntity">
/// Type of the entity.
/// </typeparam>
public interface IRepository<TEntity> : IQueryableRepository<TEntity>, IModifiableRepository<TEntity>
    where TEntity : EntityBase;
