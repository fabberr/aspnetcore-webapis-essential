using System;
using System.IO;
using Catalog.Core.Models.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Catalog.Core.Extensions;

/// <summary>
/// Provides extension methods for configuring the application.
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    /// Adds a JSON file to the configuration builder, builds the configuration, then binds the root
    /// section to an instance of <see cref="AppSettings"/> and returns it.
    /// </summary>
    /// <param name="configurationBuilder">
    /// The <see cref="IConfigurationBuilder"/> instance to which the file will
    /// be added.
    /// </param>
    /// <param name="basePath">
    /// (Optional)<br/>
    /// Absolute path to the base directory where the configuration file is located.<br/>
    /// When no value is provided <see cref="Directory.GetCurrentDirectory()"/> is used.
    /// </param>
    /// <param name="filename">
    /// (Optional)<br/>
    /// Path to the configuration file, relative to <paramref name="basePath"/>.<br/>
    /// When no value is provided <c>appsettings.json</c> is used.
    /// </param>
    /// <returns>
    /// The <see cref="AppSettings"/> instance that was bound from the root configuration section.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// If the configuration cannot be bound to to a new instance of <see cref="AppSettings"/>.
    /// </exception>
    public static AppSettings ConfigureAppSettings(
        this IConfigurationBuilder configurationBuilder,
        in string? basePath = null,
        in string? filename = null
    )
    {
        ArgumentNullException.ThrowIfNull(configurationBuilder);

        // Setup and build the configuration section
        var _basePath = basePath ?? Directory.GetCurrentDirectory();
        var _relativePath = filename ?? "appsettings.json";
        var _fullPath = Path.Join(_basePath, _relativePath);

        var configurationRoot = configurationBuilder
            .SetBasePath(_basePath)
            .AddJsonFile(
                path: _relativePath,
                optional: false,
                reloadOnChange: true
            )
            .Build();

        // Bind the root configuration to a strongly-typed `AppSettings` instance
        return configurationRoot.Get<AppSettings>(
            configureOptions: (binderOptions) => binderOptions.BindNonPublicProperties = true
        ) ?? throw new InvalidOperationException(
            message: $"Could not bind configuration to {nameof(AppSettings)} instance with configuration present in file '{_fullPath}'."
        );
    }

    /// <summary>
    /// Configures the default database connection based on the provided <see cref="AppSettings"/>.
    /// </summary>
    /// <param name="optionsBuilder">
    /// The <see cref="DbContextOptionsBuilder{TContext}"/> instance to configure.
    /// </param>
    /// <param name="appSettings">
    /// The <see cref="AppSettings"/> instance from which the default database connection settings
    /// will be read from.
    /// </param>
    /// <returns>
    /// The <paramref name="optionsBuilder"/> instance for further configuration.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If either <paramref name="optionsBuilder"/> or <paramref name="appSettings"/> is 
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// If an invalid <see cref="DatabaseProvider"/> was configured in the proided
    /// <see cref="AppSettings"/> instance.
    /// </exception>
    public static DbContextOptionsBuilder ConfigureDefaultDatabaseConnection(
        this DbContextOptionsBuilder optionsBuilder,
        in AppSettings appSettings
    )
    {
        ArgumentNullException.ThrowIfNull(optionsBuilder);
        ArgumentNullException.ThrowIfNull(appSettings);

        return appSettings.DatabaseProvider.Default switch {
            DatabaseProvider.Sqlite3 => optionsBuilder.UseSqlite(
                connectionString: appSettings.ConnectionStrings.SqlDatabase,
                sqliteOptionsAction: (sqliteOptions) => {
                    /*
                     * @todo: Map the rest of these options to AppSettings and forward them here.
                     * https://learn.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.infrastructure.relationaldbcontextoptionsbuilder-2?view=efcore-8.0
                    */
                }
            ),

            _ => throw new NotSupportedException("Invalid database provider."),
        };
    }
}
