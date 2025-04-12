using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Constants;
using Catalog.Core.Models.Entities;
using Catalog.Core.Models.Settings;
using Catalog.Core.Repositories.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Catalog.Api.Controllers;

[Route("api/[controller]")]
public sealed class CategoriesController(
    ICategoryRepository categoryRepository
)
    : CatalogApiController
{
    #region Dependencies
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    #endregion

    #region GET
    [HttpGet(Name = nameof(GetCategories))]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories(
        IOptionsSnapshot<ApiBehaviorSettings> options,
        uint? limit = null,
        uint offset = 0u
    )
    {
        var categories = await _categoryRepository.GetAsync(
            limit: limit ?? options.Value.DefaultItemsPerPage,
            offset: offset
        );

        if (categories.Any() is false)
        {
            return NoContent();
        }

        return categories.ToArray();
    }

    [HttpGet(template: "{id:int}", Name = nameof(GetCategoryById))]
    public async Task<ActionResult<Category>> GetCategoryById(
        int id
    )
    {
        if (id <= 0)
        {
            ModelState.TryAddModelError(nameof(id), string.Format(Messages.Validation.InvalidValue, id));
            return ValidationProblem(ModelState);
        }

        var category = await _categoryRepository.GetAsync(key: id);

        if (category is null)
        {
            return NotFound();
        }

        return category;
    }

    [HttpGet(template: "{id:int}/products", Name = nameof(GetCategoryProducts))]
    public async Task<ActionResult<IEnumerable<Product>>> GetCategoryProducts(
        IOptionsSnapshot<ApiBehaviorSettings> options,
        int id,
        uint? limit = null,
        uint offset = 0u
    )
    {
        if (id <= 0)
        {
            ModelState.TryAddModelError(nameof(id), string.Format(Messages.Validation.InvalidValue, id));
            return ValidationProblem(ModelState);
        }

        var products = await _categoryRepository.GetProducts(
            categoryKey: id,
            limit: limit ?? options.Value.DefaultItemsPerPage,
            offset: offset
        );

        return products.ToArray();
    }
    #endregion

    #region POST
    [HttpPost(Name = nameof(CreateCategory))]
    public async Task<ActionResult<Category>> CreateCategory(
        Category category
    )
    {
        var createdCategory = await _categoryRepository.CreateAsync(entity: category);

        return CreatedAtRoute(
            routeName: nameof(GetCategoryById),
            routeValues: new { id = category.Id },
            value: createdCategory
        );
    }
    #endregion

    #region PUT
    [HttpPut(template: "{id:int}", Name = nameof(UpdateCategoryAsync))]
    public async Task<ActionResult<Category>> UpdateCategoryAsync(
        int id,
        Category category
    )
    {
        if (id <= 0)
        {
            ModelState.TryAddModelError(nameof(id), string.Format(Messages.Validation.InvalidValue, id));
            return ValidationProblem(ModelState);
        }

        if (id != category.Id)
        {
            ModelState.TryAddModelError(nameof(id), string.Format(Messages.Validation.InvalidValue, id));
            return ValidationProblem(
                detail: string.Format(Messages.Validation.SpecifiedKeyDoesNotMatchEntityKey, id, category.Id),
                modelStateDictionary: ModelState
            );
        }

        var currentCategory = await _categoryRepository.GetAsync(key: id);
        if (currentCategory is null)
        {
            return NotFound();
        }

        if (category.Name is not "" && category.Name != currentCategory.Name)
        {
            currentCategory.Name = category.Name;
        }
        if (category.ImageUri is not "" && category.ImageUri != currentCategory.ImageUri)
        {
            currentCategory.ImageUri = category.ImageUri;
        }

        return await _categoryRepository.UpdateAsync(entity: currentCategory);
    }
    #endregion

    #region DELETE
    [HttpDelete(template: "{id:int}", Name = nameof(DeleteCategory))]
    public async Task<ActionResult<Category>> DeleteCategory(
        IOptionsSnapshot<ApiBehaviorSettings> options,
        int id
    )
    {
        if (id <= 0)
        {
            ModelState.TryAddModelError(nameof(id), string.Format(Messages.Validation.InvalidValue, id));
            return ValidationProblem(ModelState);
        }

        var deletedCategory = await _categoryRepository.DeleteAsync(
            key: id,
            strategy: options.Value.DeleteStrategy
        );

        if (deletedCategory is null)
        {
            return NotFound();
        }

        return deletedCategory;
    }
    #endregion
}

// @todo: (when DTOs are implemented) make all properties except `id` optional for PUT routes
// @todo: Paginated response DTOs for Get actions
