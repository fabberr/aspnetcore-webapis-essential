using System.Collections.Generic;
using System.Linq;
using Catalog.Core.Context;
using Catalog.Core.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Controllers
{
    [Route("api/[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiController]
    public class ProductsController(CatalogDbContext dbContext) : ControllerBase
    {
        private readonly CatalogDbContext _dbContext = dbContext;

        [HttpGet(Name = nameof(ListProducts))]
        public ActionResult<IEnumerable<Product>> ListProducts()
        {
            var products = _dbContext.Products.ToList();

            if (products is null or { Count: 0 })
            {
                return NotFound();
            }

            return products;
        }

        [HttpPost(Name = nameof(CreateProduct))]
        public ActionResult<Product> CreateProduct(Product product)
        {
            _dbContext.Add(product);
            _dbContext.SaveChanges();

            return CreatedAtRoute(
                routeName: nameof(GetProductById),
                routeValues: new { id = product.Id },
                value: product
            );
        }

        [HttpGet(template: "{id:int}", Name = nameof(GetProductById))]
        public ActionResult<Product> GetProductById(int id)
        {
            if (id <= 0)
            {
                return ValidationProblem("Invalid 'id' route parameter.");
            }

            var product = _dbContext.Products.Find(id);

            if (product is null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPut(template: "{id:int}", Name = nameof(UpdateProduct))]
        public ActionResult<Product> UpdateProduct(int id, Product product)
        {
            if (id <= 0)
            {
                return ValidationProblem("Invalid 'id' route parameter.");
            }

            if (id != product.Id)
            {
                return ValidationProblem("The 'id' route parameter does not match the entity id provided in the content.");
            }

            _dbContext.Entry(product).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return product;
        }

        [HttpDelete(template: "{id:int}", Name = nameof(DeleteProduct))]
        public ActionResult<Product> DeleteProduct(int id)
        {
            if (id <= 0)
            {
                return ValidationProblem("Invalid 'id' route parameter.");
            }

            var product = _dbContext.Products.Find(id);

            if (product is null)
            {
                return NotFound();
            }

            _dbContext.Remove(product);
            _dbContext.SaveChanges();

            return product;
        }
    }
}
