using System;
using System.Linq;
using Catalog.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Core.Context;

public class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        static bool isConcreteImplementationOfEntityBase(Type type) => (
            type.BaseType == typeof(EntityBase)
            && type is { IsAbstract: false, IsClass: true }
        );

        // Ensure every entity uses the Table Per Concrete Type (TCP) mapping strategy
        var catalogDbContextAssemblyTypes = typeof(CatalogDbContext).Assembly.GetTypes();
        foreach (var entityType in catalogDbContextAssemblyTypes.Where(isConcreteImplementationOfEntityBase))
        {
            modelBuilder.Entity(entityType).UseTpcMappingStrategy();
        }
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
}
