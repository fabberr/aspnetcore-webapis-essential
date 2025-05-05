using System;
using System.Collections.Generic;
using System.Linq;
using Catalog.Core.Models.Entities;

namespace Catalog.Core.Abstractions.Mapping;

/// <summary>
/// Static helper class providing a default implementation for
/// <see cref="IMappableFromEntityCollection{TSelf, TEntity}.FromEntities(IEnumerable{TEntity})"/>.
/// </summary>
public static class Mapper
{
    public static TSelf[]? FromEntities<TSelf, TEntity>(IEnumerable<TEntity> entities)
        where TSelf : IMappableFromEntity<TSelf, TEntity>
        where TEntity : EntityBase
    {
        ArgumentNullException.ThrowIfNull(entities);

        var convertibleEntities = entities.Where(static (entity) => entity is not null);

        if (convertibleEntities.Any() is false)
        {
            return null;
        }
        
        return [.. convertibleEntities.Select(static (entity) => TSelf.FromEntity(entity))];
    }
}
