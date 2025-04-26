using System;
using System.ComponentModel.DataAnnotations;
using Catalog.Core.Models.Entities;

namespace Catalog.Api.DTOs.Products;

public abstract record ProductDto(
    int Id = default,
    string Name = "",
    string Description = "",
    decimal Price = default,
    float Stock = default,
    string ImageUri = "",
    DateTime CreatedAt = default,
    DateTime? UpdatedAt = default
)
{
    protected ProductDto(Product product) : this (
        Id: product.Id,
        Name: product.Name,
        Description: product.Description,
        Price: product.Price,
        Stock: product.Stock,
        ImageUri: product.ImageUri,
        CreatedAt: product.CreatedAt,
        UpdatedAt: product.UpdatedAt
    )
    {}
}

#region POST
public sealed partial record CreateRequest(
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
);

public sealed partial record CreateResponse : ProductDto;
#endregion

#region GET
public sealed partial record ReadResponse : ProductDto;
#endregion

#region PUT
public sealed partial record UpdateRequest(
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
);

public sealed partial record UpdateResponse : ProductDto;
#endregion

#region PATCH
public sealed partial record PatchRequest(
    [Required]
    int Id,
    
    [StringLength(80)]
    [DeniedValues("")]
    string? Name = null,

    [StringLength(500)]
    [DeniedValues("")]
    string? Description = null,

    [DeniedValues(0)]
    decimal? Price = null,

    [DeniedValues(0f)]
    float? Stock = null,

    [StringLength(300)]
    [DeniedValues("")]
    string? ImageUri = null,

    [DeniedValues(0)]
    int? CategoryId = null
);

public sealed partial record PatchResponse : ProductDto;
#endregion

#region DELETE
public sealed partial record DeleteResponse : ProductDto;
#endregion
