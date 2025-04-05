using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Constants;
using Catalog.Core.Context;
using Catalog.Core.Models.Entities;
using Catalog.Core.Models.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Catalog.Api.Controllers;

[Route("api/[controller]")]
public sealed class CategoriesController(CatalogDbContext dbContext) : CatalogApiController
{
    #region Constants
    private const string GetCategoriesActionName        = "GetCategories";

    private const string GetCategoryByIdActionName      = "GetCategoryById";

    private const string GetCategoryProductsActionName  = "GetCategoryProducts";

    private const string CreateCategoryActionName       = "CreateCategory";

    private const string UpdateCategoryActionName       = "UpdateCategory";

    private const string DeleteCategoryActionName       = "DeleteCategory";
    #endregion

    #region Fields
    private readonly CatalogDbContext _dbContext = dbContext;
    #endregion

    #region GET
    [HttpGet(Name = GetCategoriesActionName)]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategoriesAsync(
        IOptionsSnapshot<ApiBehaviorSettings> options,
        bool includeProducts,
        uint? limit = null,
        uint offset = 0u
    )
    {
        var categoriesQuery = _dbContext.Categories.AsNoTracking()
            .OrderBy(c => c.Id)
            .Skip((int)offset)
            .Take((int)(limit ?? options.Value.DefaultItemsPerPage));

        var categories = includeProducts
            ? await categoriesQuery.Include(c => c.Products).ToArrayAsync()
            : await categoriesQuery.ToArrayAsync();

        if (categories is null or { Length: 0 })
        {
            return NotFound();
        }

        return categories;
    }

    [HttpGet(template: "{id:int}", Name = GetCategoryByIdActionName)]
    public async Task<ActionResult<Category>> GetCategoryByIdAsync(
        int id,
        bool includeProducts
    )
    {
        if (id <= 0)
        {
            ModelState.TryAddModelError(nameof(id), string.Format(Messages.Validation.InvalidValue, id));
            return ValidationProblem(ModelState);
        }

        var category = includeProducts
            ? await _dbContext.Categories.AsNoTracking()
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id)
            : await _dbContext.Categories.FindAsync(id);

        if (category is null)
        {
            return NotFound();
        }

        return category;
    }

    [HttpGet(template: "{id:int}/products", Name = GetCategoryProductsActionName)]
    public async Task<ActionResult<IEnumerable<Product>>> GetCategoryProductsAsync(
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

        var products = await _dbContext.Categories.AsNoTracking()
            .Where(c => c.Id == id)
            .Join(
                _dbContext.Products.AsNoTracking(),
                category => category.Id, product => product.CategoryId,
                (_, product) => product
            )
            .OrderBy(p => p.Id)
            .Skip((int)offset)
            .Take((int)(limit ?? options.Value.DefaultItemsPerPage))
            .ToArrayAsync();

        if (products is null or { Length: 0 })
        {
            return NotFound();
        }

        return products;
    }
    #endregion

    #region POST
    [HttpPost(Name = CreateCategoryActionName)]
    public async Task<ActionResult<Category>> CreateCategoryAsync(
        Category category
    )
    {
        await _dbContext.AddAsync(category);
        await _dbContext.SaveChangesAsync();

        return CreatedAtRoute(
            routeName: GetCategoryByIdActionName,
            routeValues: new { id = category.Id },
            value: category
        );
    }
    #endregion

    #region PUT
    [HttpPut(template: "{id:int}", Name = UpdateCategoryActionName)]
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
                detail: string.Format(Messages.Validation.ResourceIdMismatch, id, category.Id),
                modelStateDictionary: ModelState
            );
        }

        _dbContext.Entry(category).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();

        return category;
    }
    #endregion

    #region DELETE
    [HttpDelete(template: "{id:int}", Name = DeleteCategoryActionName)]
    public async Task<ActionResult<Category>> DeleteCategoryAsync(
        int id
    )
    {
        if (id <= 0)
        {
            ModelState.TryAddModelError(nameof(id), string.Format(Messages.Validation.InvalidValue, id));
            return ValidationProblem(ModelState);
        }

        var category = await _dbContext.Categories.FindAsync(id);

        if (category is null)
        {
            return NotFound();
        }

        _dbContext.Remove(category);
        await _dbContext.SaveChangesAsync();

        return category;
    }
    #endregion
}
