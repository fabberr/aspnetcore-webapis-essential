using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Catalog.Core.Context;

public sealed class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options)
{
    #region Fields
    /// <summary>
    /// A collection of types representing all entities tracked by this DbContext.
    /// </summary>
    private readonly static IEnumerable<Type> _entityTypes;
    #endregion

    #region Initialization
    static CatalogDbContext()
    {
        // Predicates
        static bool isConcreteImplementationOfEntityBase(Type type) => (
            type.BaseType == typeof(EntityBase)
            && type is { IsAbstract: false, IsClass: true }
        );

        var catalogDbContextAssemblyTypes = typeof(CatalogDbContext).Assembly.GetTypes();
        _entityTypes = catalogDbContextAssemblyTypes.Where(isConcreteImplementationOfEntityBase);
    }
    #endregion

    #region Private Methods
    private void SetUpdatedAtTimestampForModifiedEntities()
    {
        var modifiedEntities = ChangeTracker.Entries()
            .Where(entry => entry.State is EntityState.Modified)
            .Select(entry => entry.Entity)
            .ToList();

        foreach (var entity in modifiedEntities)
        {
            (entity as EntityBase)!.UpdatedAt = DateTime.UtcNow;
        }
    }
    #endregion

    #region OnModelCreating
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Ensure every entity uses the Table Per Concrete Type (TCP) mapping strategy
        foreach (var entityType in _entityTypes)
        {
            modelBuilder.Entity(entityType).UseTpcMappingStrategy();
        }
    }
    #endregion

    #region SaveChanges
    /// <inheritdoc />
    public override int SaveChanges()
    {
        SetUpdatedAtTimestampForModifiedEntities();
        return base.SaveChanges();
    }

    /// <inheritdoc />
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        SetUpdatedAtTimestampForModifiedEntities();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }
    #endregion

    #region SaveChangesAsync
    /// <inheritdoc />
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetUpdatedAtTimestampForModifiedEntities();
        return base.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        SetUpdatedAtTimestampForModifiedEntities();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
    #endregion

    #region DBSets
    public DbSet<Category> Categories { get; set; }
    
    public DbSet<Product> Products { get; set; }
    #endregion
}
