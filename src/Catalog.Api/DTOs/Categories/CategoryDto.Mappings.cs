using System;
using System.Collections.Generic;
using Catalog.Core.Models.Abstractions;
using Catalog.Core.Models.Entities;

namespace Catalog.Api.DTOs.Categories;

#region GET
public sealed partial record ReadResponse
    : IMappableFromEntity<ReadResponse, Category>
    , IMappableFromEntityCollection<ReadResponse, Category>
{
    public static ReadResponse FromEntity(Category entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new ReadResponse() {
            Id = entity.Id,
            Name = entity.Name,
            ImageUri = entity.ImageUri,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
        };
    }

    public static ReadResponse[]? FromEntities(IEnumerable<Category> sources)
    {
        return Mapper.FromEntities<ReadResponse, Category>(sources);
    }
}
#endregion

#region POST
public sealed partial record CreateRequest
    : IMappableToEntity<Category>
{
    public Category ToEntity() => new() {
        Name = Name,
        ImageUri = ImageUri,
    };
}

public sealed partial record CreateResponse
    : IMappableFromEntity<CreateResponse, Category>
{
    public static CreateResponse FromEntity(Category entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new CreateResponse() {
            Id = entity.Id,
            Name = entity.Name,
            ImageUri = entity.ImageUri,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
        };
    }
}
#endregion

#region PUT
public sealed partial record UpdateRequest
    : IMappableToEntity<Category>
{
    public Category ToEntity() => new() {
        Id = Id,
        Name = Name,
        ImageUri = ImageUri,
    };
}

public sealed partial record UpdateResponse
    : IMappableFromEntity<UpdateResponse, Category>
{
    public static UpdateResponse FromEntity(Category entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        return new UpdateResponse() {
            Id = entity.Id,
            Name = entity.Name,
            ImageUri = entity.ImageUri,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
        };
    }
}
#endregion

#region DELETE
public sealed partial record DeleteResponse
    : IMappableFromEntity<DeleteResponse, Category>
{
    public static DeleteResponse FromEntity(Category entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new DeleteResponse() {
            Id = entity.Id,
            Name = entity.Name,
            ImageUri = entity.ImageUri,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
        };
    }
}
#endregion
