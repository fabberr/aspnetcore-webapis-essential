using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Catalog.Core.Models.Abstractions;
using Catalog.Core.Models.Entities;

namespace Catalog.Api.DTOs.Products;

public abstract record class ProductDto(
    int Id = default,
    string Name = "",
    string Description = "",
    decimal Price = default,
    float Stock = default,
    string ImageUri = "",
    DateTime CreatedAt = default,
    DateTime? UpdatedAt = default
);

#region GET
public sealed record class ReadResponse : ProductDto
    , IMappableFromEntity<ReadResponse, Product>
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
public sealed record class CreateRequest(
    [Required]
    [StringLength(80)]
    string Name,

    [Required]
    [StringLength(500)]
    string Description,

    [Required]
    decimal Price,

    [Required]
    [StringLength(300)]
    string ImageUri,

    [Required]
    int CategoryId
)
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

public sealed record class CreateResponse : ProductDto
    , IMappableFromEntity<CreateResponse, Product>
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
public sealed record class UpdateRequest(
    [Required]
    int Id,
    
    [Required]
    [StringLength(80)]
    string Name,

    [Required]
    [StringLength(500)]
    string Description,

    [Required]
    decimal Price,

    [Required]
    float Stock,

    [Required]
    [StringLength(300)]
    string ImageUri,

    [Required]
    int CategoryId
)
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

public sealed record class UpdateResponse : ProductDto
    , IMappableFromEntity<UpdateResponse, Product>
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
public sealed record class DeleteResponse : ProductDto
    , IMappableFromEntity<DeleteResponse, Product>
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
