using System.Linq;
using Catalog.Core.Factories;
using Catalog.Core.Models.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Core.Migrations
{
    /// <inheritdoc />
    public partial class SeedTableProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            using var catalogDbContext = new CatalogDbContextFactory()
                .CreateDbContext(args: null);
            
            var categories = catalogDbContext.Categories.ToList();
            var drinksCategory = categories.Single(c => c.Name == "Drinks");
            var snacksCategory = categories.Single(c => c.Name == "Snacks");
            var dessertsCategory = categories.Single(c => c.Name == "Desserts");

            catalogDbContext.AddRange([
                /* DRINKS */
                new Product() {
                    CategoryId = drinksCategory.Id,
                    Name = "Milk 2 L",
                    Description = "Milk jug - 2 L",
                    Price = 07.5m,
                    Stock = 100f,
                    ImageUri = "milk.jpeg"
                },
                new Product() {
                    CategoryId = drinksCategory.Id,
                    Name = "Orange juice 1 L",
                    Description = "Orange juice box - 1 L",
                    Price = 08.9m,
                    Stock = 85f,
                    ImageUri = "orange-juice.jpeg"
                },

                /* SNACKS */
                new Product() {
                    CategoryId = snacksCategory.Id,
                    Name = "Peanuts 1 kg",
                    Description = "Roasted peanuts packet - Large 1 kg",
                    Price = 22.5m,
                    Stock = 120f,
                    ImageUri = ""
                },
                new Product() {
                    CategoryId = snacksCategory.Id,
                    Name = "Sandwich",
                    Description = "Natural sandwich - cheese, ham, lettuce and tomato",
                    Price = 21.9m,
                    Stock = 12f,
                    ImageUri = "sandwich.jpeg"
                },

                /* DESSERTS */
                new Product() {
                    CategoryId = dessertsCategory.Id,
                    Name = "Pudding 85g",
                    Description = "Condensed milk pudding - 1 serving 85g",
                    Price = 12.75m,
                    Stock = 25f,
                    ImageUri = "pudding.jpeg"
                },
                new Product() {
                    CategoryId = dessertsCategory.Id,
                    Name = "Dark Chocolate 125g",
                    Description = "Dark chocolate bar - 125g",
                    Price = 15.9m,
                    Stock = 90f,
                    ImageUri = "chocolate.jpeg"
                },
            ]);

            catalogDbContext.SaveChanges();
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            using var catalogDbContext = new CatalogDbContextFactory()
                .CreateDbContext(args: null);

            var existingProducts = catalogDbContext.Products.ToList();
            catalogDbContext.RemoveRange(existingProducts);

            catalogDbContext.SaveChanges();
        }
    }
}
