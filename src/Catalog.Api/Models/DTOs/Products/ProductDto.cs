using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Catalog.Core.Models.Entities;

namespace Catalog.Api.Models.DTOs.Products;

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
public sealed record class GetProductResponse : ProductDto
{
    public static explicit operator GetProductResponse(Product product) => new() {
        Id = product.Id,
        Name = product.Name,
        Description = product.Description,
        Price = product.Price,
        Stock = product.Stock,
        ImageUri = product.ImageUri,
        CreatedAt = product.CreatedAt,
        UpdatedAt = product.UpdatedAt,
    };
}
#endregion

#region POST
public sealed record class CreateProductRequest(
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
{
    public static explicit operator Product(CreateProductRequest createProductRequest) => new() {
        Name = createProductRequest.Name,
        Description  = createProductRequest.Description,
        Price = createProductRequest.Price,
        Stock = 0f,
        ImageUri = createProductRequest.ImageUri,
        CategoryId = createProductRequest.CategoryId,
    };
}

public sealed record class CreateProductResponse : ProductDto
{
    public static explicit operator CreateProductResponse(Product product) => new() {
        Id = product.Id,
        Name = product.Name,
        Description = product.Description,
        Price = product.Price,
        Stock = product.Stock,
        ImageUri = product.ImageUri,
        CreatedAt = product.CreatedAt,
        UpdatedAt = product.UpdatedAt,
    };
}
#endregion

#region PUT
public sealed record class UpdateProductRequest(
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
{
    public static explicit operator Product(UpdateProductRequest updateProductRequest) => new() {
        Id = updateProductRequest.Id,
        Name = updateProductRequest.Name,
        Description = updateProductRequest.Description,
        Price = updateProductRequest.Price,
        Stock = updateProductRequest.Stock,
        ImageUri = updateProductRequest.ImageUri,
        CategoryId = updateProductRequest.CategoryId,
    };
}

public sealed record class UpdateProductResponse : ProductDto
{
    public static explicit operator UpdateProductResponse(Product product) => new() {
        Id = product.Id,
        Name = product.Name,
        Description = product.Description,
        Price = product.Price,
        Stock = product.Stock,
        ImageUri = product.ImageUri,
        CreatedAt = product.CreatedAt,
        UpdatedAt = product.UpdatedAt,
    };
}
#endregion

#region DELETE
public sealed record class DeleteProductResponse : ProductDto
{
    public static explicit operator DeleteProductResponse(Product product) => new() {
        Id = product.Id,
        Name = product.Name,
        Description = product.Description,
        Price = product.Price,
        Stock = product.Stock,
        ImageUri = product.ImageUri,
        CreatedAt = product.CreatedAt,
        UpdatedAt = product.UpdatedAt,
    };
}

#endregion
