using System.Collections.Generic;
using Catalog.Core.Models.Entities;
using Catalog.Core.Models.Entities.Abstractions;

namespace Catalog.Api.DTOs.Products;

#region POST
public sealed partial record CreateRequest : IMappableToEntity<Product>
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

public sealed partial record CreateResponse : IMappableFromEntity<CreateResponse, Product>
{
    private CreateResponse(Product product) : base(product) {}

    public static CreateResponse FromEntity(Product entity) => new(entity);
}
#endregion

#region GET
public sealed partial record ReadResponse
    : IMappableFromEntity<ReadResponse, Product>
    , IMappableFromEntityCollection<ReadResponse, Product>
{
    private ReadResponse(Product product) : base(product) {}

    public static ReadResponse FromEntity(Product entity) => new(entity);

    public static ReadResponse[]? FromEntities(IEnumerable<Product> sources) => Mapper.FromEntities<ReadResponse, Product>(sources);
}
#endregion

#region PUT
public sealed partial record UpdateRequest : IMappableToEntity<Product>
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

public sealed partial record UpdateResponse : IMappableFromEntity<UpdateResponse, Product>
{
    private UpdateResponse(Product product) : base(product) {}

    public static UpdateResponse FromEntity(Product entity) => new(entity);
}
#endregion

#region PATCH
public sealed partial record PatchRequest : IMappableToEntity<Product>
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

public sealed partial record PatchResponse : IMappableFromEntity<PatchResponse, Product>
{
    private PatchResponse(Product product) : base(product) {}

    public static PatchResponse FromEntity(Product entity) => new(entity);
}
#endregion

#region DELETE
public sealed partial record DeleteResponse : IMappableFromEntity<DeleteResponse, Product>
{
    private DeleteResponse(Product product) : base(product) {}

    public static DeleteResponse FromEntity(Product entity) => new(entity);
}
#endregion
