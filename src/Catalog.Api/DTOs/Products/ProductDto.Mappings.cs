using System;
using System.Collections.Generic;
using System.Linq;
using Catalog.Core.Models.Abstractions;
using Catalog.Core.Models.Entities;

namespace Catalog.Api.DTOs.Products;

#region GET
public sealed partial record ReadResponse
    : IMappableFromEntity<ReadResponse, Product>
    , IMappableFromEntityCollection<ReadResponse, Product>
{
    public static ReadResponse FromEntity(Product source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new ReadResponse() {
            Id = source.Id,
            Name = source.Name,
            Description = source.Description,
            Price = source.Price,
            Stock = source.Stock,
            ImageUri = source.ImageUri,
            CreatedAt = source.CreatedAt,
            UpdatedAt = source.UpdatedAt,
        };
    }

    public static ReadResponse[]? FromEntities(IEnumerable<Product> sources)
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
    : IMappableToEntity<Product>
{
    public Product ToEntity()
    {
        return new Product() {
            Name = this.Name,
            Description  = this.Description,
            Price = this.Price,
            Stock = 0f,
            ImageUri = this.ImageUri,
            CategoryId = this.CategoryId,
        };
    }
}

public sealed partial record CreateResponse
    : IMappableFromEntity<CreateResponse, Product>
{
    public static CreateResponse FromEntity(Product source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new CreateResponse() {
            Id = source.Id,
            Name = source.Name,
            Description = source.Description,
            Price = source.Price,
            Stock = source.Stock,
            ImageUri = source.ImageUri,
            CreatedAt = source.CreatedAt,
            UpdatedAt = source.UpdatedAt,
        };
    }
}
#endregion

#region PUT
public sealed partial record UpdateRequest
    : IMappableToEntity<Product>
{
    public Product ToEntity()
    {
        return new Product() {
            Id = this.Id,
            Name = this.Name,
            Description = this.Description,
            Price = this.Price,
            Stock = this.Stock,
            ImageUri = this.ImageUri,
            CategoryId = this.CategoryId,
        };
    }
}

public sealed partial record UpdateResponse
    : IMappableFromEntity<UpdateResponse, Product>
{
    public static UpdateResponse FromEntity(Product source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new UpdateResponse() {
            Id = source.Id,
            Name = source.Name,
            Description = source.Description,
            Price = source.Price,
            Stock = source.Stock,
            ImageUri = source.ImageUri,
            CreatedAt = source.CreatedAt,
            UpdatedAt = source.UpdatedAt,
        };
    }
}
#endregion

#region DELETE
public sealed partial record DeleteResponse
    : IMappableFromEntity<DeleteResponse, Product>
{
    public static DeleteResponse FromEntity(Product source)
    {
        ArgumentNullException.ThrowIfNull(source);
        
        return new DeleteResponse() {
            Id = source.Id,
            Name = source.Name,
            Description = source.Description,
            Price = source.Price,
            Stock = source.Stock,
            ImageUri = source.ImageUri,
            CreatedAt = source.CreatedAt,
            UpdatedAt = source.UpdatedAt,
        };
    }
}
#endregion
