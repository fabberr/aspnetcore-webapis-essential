using System;
using System.Collections.Generic;
using System.Linq;
using Catalog.Core.Models.Entities;

namespace Catalog.Api.Models.DTOs.Categories;

internal static class CategoryDtoMappingExtensions
{
    #region GET
    internal static GetCategoryResponse[] ToGetResponse(this IEnumerable<Category> categories)
    {
        ArgumentNullException.ThrowIfNull(categories);

        var convertibleCategories = categories.Where(static (category) => category is not null);
        
        return [.. convertibleCategories.Select(static (category) => (GetCategoryResponse)category)];
    }

    internal static GetCategoryResponse ToGetResponse(this Category category)
    {
        ArgumentNullException.ThrowIfNull(category);

        return (GetCategoryResponse)category;
    }
    #endregion

    #region POST
    internal static Category ToEntity(this CreateCategoryRequest createCategoryRequest)
    {
        ArgumentNullException.ThrowIfNull(createCategoryRequest);

        return (Category)createCategoryRequest;
    }

    internal static CreateCategoryResponse ToCreateResponse(this Category category)
    {
        ArgumentNullException.ThrowIfNull(category);

        return (CreateCategoryResponse)category;
    }
    #endregion

    #region PUT
    internal static Category ToEntity(this UpdateCategoryRequest updateCategoryRequest)
    {
        ArgumentNullException.ThrowIfNull(updateCategoryRequest);

        return (Category)updateCategoryRequest;
    }

    internal static UpdateCategoryResponse ToUpdateResponse(this Category category)
    {
        ArgumentNullException.ThrowIfNull(category);

        return (UpdateCategoryResponse)category;
    }
    #endregion

    #region DELETE
    internal static DeleteCategoryResponse ToDeleteResponse(this Category category)
    {
        ArgumentNullException.ThrowIfNull(category);

        return (DeleteCategoryResponse)category;
    }
    #endregion
}
