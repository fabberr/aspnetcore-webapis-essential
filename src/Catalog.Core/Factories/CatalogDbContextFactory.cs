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
    private const string EnvironmentVariablePrefix      = "ASPNETCORE_";
    private const string ConfigurationBaseDirectoryKey  = "CONFIGURATION_BASE_DIRECTORY";
    private const string ConfigurationFilenameKey       = "CONFIGURATION_FILENAME";

    private readonly AppSettings _appSettings;

    /// <summary>
    /// Initializes a new instance of <see cref="CatalogDbContextFactory"/>.<br/>
    /// An <see cref="AppSettings"/> instance will be configured through the
    /// <see cref="ConfigurationExtensions.ConfigureAppSettings"/> method.
    /// </summary>
    public CatalogDbContextFactory()
    {
        var environmentVariables = new ConfigurationBuilder()
            .AddEnvironmentVariables(prefix: EnvironmentVariablePrefix)
            .Build();

        _appSettings = new ConfigurationBuilder()
            .ConfigureAppSettings(
                basePath: environmentVariables[ConfigurationBaseDirectoryKey],
                filename: environmentVariables[ConfigurationFilenameKey]
            );
    }

    /// <inheritdoc />
    public CatalogDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>();
        optionsBuilder.ConfigureDefaultDatabaseConnection(_appSettings);

        return new CatalogDbContext(optionsBuilder.Options);
    }
}
