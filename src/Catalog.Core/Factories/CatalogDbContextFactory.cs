using Catalog.Core.Context;
using Catalog.Core.Extensions;
using Catalog.Core.Models.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Catalog.Core.Factories;

/// <summary>
/// Provides the <c>dotnet-ef</c> CLI tools with a method for creating design-time
/// instances of <see cref="CatalogDbContext"/>.
/// </summary>
public sealed class CatalogDbContextFactory : IDesignTimeDbContextFactory<CatalogDbContext>
{
    private readonly IAppSettings _appSettings;

    /// <summary>
    /// Initializes a new instance of <see cref="CatalogDbContextFactory"/> with
    /// a <see cref="IAppSettings"/>instance bound from a JSON file.
    /// </summary>
    public CatalogDbContextFactory() => _appSettings = new ConfigurationBuilder().ConfigureAppSettings();

    /// <inheritdoc />
    public CatalogDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>();
        optionsBuilder.ConfigureDefaultDatabaseConnection(_appSettings);

        return new CatalogDbContext(optionsBuilder.Options);
    }
}
