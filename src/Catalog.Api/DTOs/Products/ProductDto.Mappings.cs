using System.Collections.Generic;
using Catalog.Core.Abstractions.Mapping;
using Catalog.Core.Models.Entities;

namespace Catalog.Api.DTOs.Products;

public abstract partial record ProductDto
{
    protected ProductDto(
        Product product
    ) : this (
        Id: product.Id,
        Name: product.Name,
        Description: product.Description,
        Price: product.Price,
        Stock: product.Stock,
        ImageUri: product.ImageUri,
        CreatedAt: product.CreatedAt,
        UpdatedAt: product.UpdatedAt
    ) {}
}

#region POST
public sealed partial record CreateProductRequest : IMappableToEntity<Product>
{
    public Product ToEntity() => new() {
        Name = this.Name,
        Description = this.Description,
        Price = this.Price,
        Stock = 0f,
        ImageUri = this.ImageUri,
        CategoryId = this.CategoryId,
    };
}

public sealed partial record CreateProductResponse : IMappableFromEntity<CreateProductResponse, Product>
{
    private CreateProductResponse(Product product) : base(product) {}

    public static CreateProductResponse FromEntity(Product entity) => new(entity);
}
#endregion

#region GET
public sealed partial record ReadProductResponse
    : IMappableFromEntity<ReadProductResponse, Product>
    , IMappableFromEntityCollection<ReadProductResponse, Product>
{
    private ReadProductResponse(Product product) : base(product) {}

    public static ReadProductResponse FromEntity(Product entity) => new(entity);

    public static ReadProductResponse[]? FromEntities(IEnumerable<Product> sources) => Mapper.FromEntities<ReadProductResponse, Product>(sources);
}
#endregion

#region PUT
public sealed partial record UpdateProductRequest : IMappableToEntity<Product>
{
    public Product ToEntity() => new() {
        Id = this.Id,
        Name = this.Name,
        Description = this.Description,
        Price = this.Price,
        Stock = this.Stock,
        ImageUri = this.ImageUri,
        CategoryId = this.CategoryId,
    };
}

public sealed partial record UpdateProductResponse : IMappableFromEntity<UpdateProductResponse, Product>
{
    private UpdateProductResponse(Product product) : base(product) {}

    public static UpdateProductResponse FromEntity(Product entity) => new(entity);
}
#endregion

#region PATCH
public sealed partial record PatchProductRequest : IMappableToEntity<Product>
{
    public Product ToEntity() => new() {
        Id = Id,
        Name = Name!,
        Description = Description!,
        Price = Price.GetValueOrDefault(),
        Stock = Stock.GetValueOrDefault(),
        ImageUri = ImageUri!,
        CategoryId = CategoryId.GetValueOrDefault(),
    };
}

public sealed partial record PatchProductResponse : IMappableFromEntity<PatchProductResponse, Product>
{
    private PatchProductResponse(Product product) : base(product) {}

    public static PatchProductResponse FromEntity(Product entity) => new(entity);
}
#endregion

#region DELETE
public sealed partial record DeleteProductResponse : IMappableFromEntity<DeleteProductResponse, Product>
{
    private DeleteProductResponse(Product product) : base(product) {}

    public static DeleteProductResponse FromEntity(Product entity) => new(entity);
}
#endregion
