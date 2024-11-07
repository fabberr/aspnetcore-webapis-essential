using System;
using System.Collections.Generic;
using System.Linq;
using Catalog.Core.Context;
using Catalog.Core.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Controllers;

[Route("api/[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
[ApiController]
public class CategoriesController(CatalogDbContext dbContext) : ControllerBase
{
    private readonly CatalogDbContext _dbContext = dbContext;

    [HttpGet(Name = nameof(ListCategories))]
    public ActionResult<IEnumerable<Category>> ListCategories(bool includeProducts, uint limit = 10u, uint offset = 0u)
    {
        var categoriesQuery = _dbContext.Categories.AsNoTracking()
            .Skip((int)offset)
            .Take((int)limit);

        var categories = includeProducts
            ? categoriesQuery.Include(c => c.Products).ToList()
            : categoriesQuery.ToList();

        if (categories is null or { Count: 0 })
        {
            return NotFound();
        }

        return categories;
    }

    [HttpPost(Name = nameof(CreateCategory))]
    public ActionResult<Category> CreateCategory(Category category)
    {
        _dbContext.Add(category);
        _dbContext.SaveChanges();

        return CreatedAtRoute(
            routeName: nameof(GetCategoryById),
            routeValues: new { id = category.Id },
            value: category
        );
    }

    [HttpGet(template: "{id:int}", Name = nameof(GetCategoryById))]
    public ActionResult<Category> GetCategoryById(int id, bool includeProducts)
    {
        if (id <= 0)
        {
            return ValidationProblem("Invalid 'id' route parameter.");
        }

        var category = includeProducts
            ? _dbContext.Categories.AsNoTracking().Include(c => c.Products).FirstOrDefault(c => c.Id == id)
            : _dbContext.Categories.Find(id);

        if (category is null)
        {
            return NotFound();
        }

        return category;
    }

    [HttpGet(template: "{id:int}/products", Name = nameof(ListCategoryProducts))]
    public ActionResult<IEnumerable<Product>> ListCategoryProducts(int id, uint limit = 10u, uint offset = 0u)
    {
        if (id <= 0)
        {
            return ValidationProblem("Invalid 'id' route parameter.");
        }

        var products = _dbContext.Categories.AsNoTracking()
            .Where(c => c.Id == id)
            .Join(
                _dbContext.Products.AsNoTracking(),
                category => category.Id, product => product.CategoryId,
                (_, product) => product
            )
            .Skip((int)offset)
            .Take((int)limit)
            .ToList();

        if (products is null or { Count: 0 })
        {
            return NotFound();
        }

        return products.ToList();
    }

    [HttpPut(template: "{id:int}", Name = nameof(UpdateCategory))]
    public ActionResult<Category> UpdateCategory(int id, Category category)
    {
        if (id <= 0)
        {
            return ValidationProblem("Invalid 'id' route parameter.");
        }

        if (id != category.Id)
        {
            return ValidationProblem("The 'id' route parameter does not match the entity id provided in the content.");
        }

        _dbContext.Entry(category).State = EntityState.Modified;
        _dbContext.SaveChanges();

        return category;
    }

    [HttpDelete(template: "{id:int}", Name = nameof(DeleteCategory))]
    public ActionResult<Category> DeleteCategory(int id)
    {
        if (id <= 0)
        {
            return ValidationProblem("Invalid 'id' route parameter.");
        }

        var category = _dbContext.Categories.Find(id);

        if (category is null)
        {
            return NotFound();
        }

        _dbContext.Remove(category);
        _dbContext.SaveChanges();

        return category;
    }
}
