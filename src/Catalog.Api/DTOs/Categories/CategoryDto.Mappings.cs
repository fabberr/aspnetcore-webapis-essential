using System;
using System.Collections.Generic;
using System.Linq;
using Catalog.Core.Models.Abstractions;
using Catalog.Core.Models.Entities;

namespace Catalog.Api.DTOs.Categories;

#region GET
public sealed partial record ReadResponse
    : IMappableFromEntity<ReadResponse, Category>
    , IMappableFromEntityCollection<ReadResponse, Category>
{
    public static ReadResponse FromEntity(Category source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new ReadResponse() {
            Id = source.Id,
            Name = source.Name,
            ImageUri = source.ImageUri,
            CreatedAt = source.CreatedAt,
            UpdatedAt = source.UpdatedAt,
        };
    }

    public static ReadResponse[]? FromEntities(IEnumerable<Category> sources)
    {
        ArgumentNullException.ThrowIfNull(sources);

        var convertibleSources = sources.Where(static (source) => source is not null);

        if (convertibleSources.Any() is false)
        {
            return null;
        }
        
        return [.. convertibleSources.Select(static (source) => FromEntity(source))];
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
    public static CreateResponse FromEntity(Category source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new CreateResponse() {
            Id = source.Id,
            Name = source.Name,
            ImageUri = source.ImageUri,
            CreatedAt = source.CreatedAt,
            UpdatedAt = source.UpdatedAt,
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
    public static UpdateResponse FromEntity(Category source)
    {
        ArgumentNullException.ThrowIfNull(source);
        
        return new UpdateResponse() {
            Id = source.Id,
            Name = source.Name,
            ImageUri = source.ImageUri,
            CreatedAt = source.CreatedAt,
            UpdatedAt = source.UpdatedAt,
        };
    }
}
#endregion

#region DELETE
public sealed partial record DeleteResponse
    : IMappableFromEntity<DeleteResponse, Category>
{
    public static DeleteResponse FromEntity(Category source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new DeleteResponse() {
            Id = source.Id,
            Name = source.Name,
            ImageUri = source.ImageUri,
            CreatedAt = source.CreatedAt,
            UpdatedAt = source.UpdatedAt,
        };
    }
}
#endregion
