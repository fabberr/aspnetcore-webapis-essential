using System;
using System.ComponentModel.DataAnnotations;
using Catalog.Core.Models.Entities;

namespace Catalog.Api.DTOs.Categories;

public abstract record CategoryDto(
    int Id = default,
    string Name = "",
    string ImageUri = "",
    DateTime CreatedAt = default,
    DateTime? UpdatedAt = default
)
{
    protected CategoryDto(Category category) : this(
        Id: category.Id,
        Name: category.Name,
        ImageUri: category.ImageUri,
        CreatedAt: category.CreatedAt,
        UpdatedAt: category.UpdatedAt
    )
    {}
}

#region POST
public sealed partial record CreateCategoryRequest(
    [Required]
    [StringLength(80)]
    string Name,

    [Required]
    [StringLength(300)]
    string ImageUri
);

public sealed partial record CreateCategoryResponse : CategoryDto;
#endregion

#region GET
public sealed partial record ReadCategoryResponse : CategoryDto;
#endregion

#region PUT
public sealed partial record UpdateCategoryRequest(
    [Required]
    int Id,

    [Required]
    [StringLength(80)]
    string Name,

    [Required]
    [StringLength(300)]
    string ImageUri
);

public sealed partial record UpdateCategoryResponse : CategoryDto;
#endregion

#region PATCH
public sealed partial record PatchCategoryRequest(
    [Required]
    int Id,

    [StringLength(80)]
    [DeniedValues("")]
    string? Name = null,

    [StringLength(300)]
    [DeniedValues("")]
    string? ImageUri = null
);

public sealed partial record PatchCategoryResponse : CategoryDto;
#endregion

#region DELETE
public sealed partial record DeleteCategoryResponse : CategoryDto;
#endregion
