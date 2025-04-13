using System;
using System.ComponentModel.DataAnnotations;
using Catalog.Core.Models.Entities;

namespace Catalog.Api.Models.DTOs.Categories;

public record class CategoryDto(
    int Id = default,
    string Name = "",
    string ImageUri = "",
    DateTime CreatedAt = default,
    DateTime? UpdatedAt = default
);

#region GET
public sealed record class GetCategoryResponse : CategoryDto
{
    public static explicit operator GetCategoryResponse(Category category) => new() {
        Id = category.Id,
        Name = category.Name,
        ImageUri = category.ImageUri,
        CreatedAt = category.CreatedAt,
        UpdatedAt = category.UpdatedAt,
    };
}
#endregion

#region POST
public sealed record class CreateCategoryRequest(
    [Required]
    string Name,

    [Required]
    string ImageUri
)
{
    public static explicit operator Category(CreateCategoryRequest createCategoryRequest) => new() {
        Name = createCategoryRequest.Name,
        ImageUri = createCategoryRequest.ImageUri,
    };
}

public sealed record class CreateCategoryResponse : CategoryDto
{
    public static explicit operator CreateCategoryResponse(Category category) => new() {
        Id = category.Id,
        Name = category.Name,
        ImageUri = category.ImageUri,
        CreatedAt = category.CreatedAt,
        UpdatedAt = category.UpdatedAt,
    };
}
#endregion

#region PUT
public sealed record class UpdateCategoryRequest(
    [Required]
    int Id,

    [Required]
    string Name,

    [Required]
    string ImageUri
)
{
    public static explicit operator Category(UpdateCategoryRequest updateCategoryRequest) => new() {
        Id = updateCategoryRequest.Id,
        Name = updateCategoryRequest.Name,
        ImageUri = updateCategoryRequest.ImageUri,
    };
}

public sealed record class UpdateCategoryResponse : CategoryDto
{
    public static explicit operator UpdateCategoryResponse(Category category) => new() {
        Id = category.Id,
        Name = category.Name,
        ImageUri = category.ImageUri,
        CreatedAt = category.CreatedAt,
        UpdatedAt = category.UpdatedAt,
    };
}
#endregion

#region DELETE
public sealed record class DeleteCategoryResponse() : CategoryDto
{
    public static explicit operator DeleteCategoryResponse(Category category) => new() {
        Id = category.Id,
        Name = category.Name,
        ImageUri = category.ImageUri,
        CreatedAt = category.CreatedAt,
        UpdatedAt = category.UpdatedAt,
    };
}
#endregion
