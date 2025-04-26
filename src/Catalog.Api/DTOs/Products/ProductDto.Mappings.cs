using System;
using System.Collections.Generic;
using Catalog.Core.Models.Abstractions;
using Catalog.Core.Models.Entities;

namespace Catalog.Api.DTOs.Products;

#region GET
public sealed partial record ReadResponse
    : IMappableFromEntity<ReadResponse, Product>
    , IMappableFromEntityCollection<ReadResponse, Product>
{
    public static ReadResponse FromEntity(Product entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new ReadResponse() {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Price = entity.Price,
            Stock = entity.Stock,
            ImageUri = entity.ImageUri,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
        };
    }

    public static ReadResponse[]? FromEntities(IEnumerable<Product> sources)
    {
        return Mapper.FromEntities<ReadResponse, Product>(sources);
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
    public static CreateResponse FromEntity(Product entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new CreateResponse() {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Price = entity.Price,
            Stock = entity.Stock,
            ImageUri = entity.ImageUri,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
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
    public static UpdateResponse FromEntity(Product entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new UpdateResponse() {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Price = entity.Price,
            Stock = entity.Stock,
            ImageUri = entity.ImageUri,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
        };
    }
}
#endregion

#region DELETE
public sealed partial record DeleteResponse
    : IMappableFromEntity<DeleteResponse, Product>
{
    public static DeleteResponse FromEntity(Product entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        return new DeleteResponse() {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Price = entity.Price,
            Stock = entity.Stock,
            ImageUri = entity.ImageUri,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
        };
    }
}
#endregion
