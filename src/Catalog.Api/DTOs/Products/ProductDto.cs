using System;
using System.ComponentModel.DataAnnotations;

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
public sealed partial record ReadResponse : ProductDto;
#endregion

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

#region DELETE
public sealed partial record DeleteResponse : ProductDto;
#endregion
