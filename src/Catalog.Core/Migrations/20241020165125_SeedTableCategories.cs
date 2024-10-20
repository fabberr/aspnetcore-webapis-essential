using System.Linq;
using Catalog.Core.Factories;
using Catalog.Core.Models.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Core.Migrations
{
    /// <inheritdoc />
    public partial class SeedTableCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            using var catalogDbContext = new CatalogDbContextFactory()
                .CreateDbContext(args: null);

            catalogDbContext.AddRange([
                new Category() { Name = "Drinks", ImageUri = "drinks.jpeg" },
                new Category() { Name = "Snacks", ImageUri = "snacks.jpeg" },
                new Category() { Name = "Desserts", ImageUri = "desserts.jpeg" },
            ]);

            catalogDbContext.SaveChanges();
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            using var catalogDbContext = new CatalogDbContextFactory()
                .CreateDbContext(args: null);

            var existingCategories = catalogDbContext.Categories.ToList();
            catalogDbContext.RemoveRange(existingCategories);

            catalogDbContext.SaveChanges();
        }
    }
}
