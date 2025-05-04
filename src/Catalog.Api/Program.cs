using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Catalog.Api.Extensions;
using Catalog.Api.Factories;
using Catalog.Core.Attributes;
using Catalog.Core.Context;
using Catalog.Core.Extensions;
using Catalog.Core.Models.Settings;
using Catalog.Core.Repositories.Abstractions;
using Catalog.Core.Repositories.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

#region Configuration
var appSettings = builder.Configuration.ConfigureAppSettings();

builder.Services
    .Configure<RouteOptions>(options => {
        options.LowercaseUrls = true;
        options.LowercaseQueryStrings = true;
    });

// Locate the `Configure<TOptions>(this IServiceCollection, IConfiguration)` method by its signature.
var configureMethod = typeof(OptionsConfigurationServiceCollectionExtensions).GetMethod(
    name: nameof(OptionsConfigurationServiceCollectionExtensions.Configure),
    types: [ typeof(IServiceCollection), typeof(IConfiguration) ]
)!;

// Dynamically configure instances of `IOptions<T>` for each section
var configurationSectionTypes = typeof(AppSettings).Assembly.GetExportedTypes()
    .Where(type => type.GetCustomAttribute<ConfigurationSectionAttribute>() is not null);

foreach (var type in configurationSectionTypes)
{
    var sectionName = type.GetCustomAttribute<ConfigurationSectionAttribute>()!.SectionName;

    configureMethod.MakeGenericMethod(type)
        .Invoke(obj: null, parameters: [
            builder.Services,
            builder.Configuration.GetSection(sectionName)
        ]
    );
}
#endregion

#region Services
builder.Services
    .AddDbContext<DbContext, CatalogDbContext>(
        optionsAction: (options) => options
            .ConfigureDefaultDatabaseConnection(appSettings),
        contextLifetime: ServiceLifetime.Scoped,
        optionsLifetime: ServiceLifetime.Scoped
    );

builder.Services
    .AddScoped<ICategoryRepository, CategoryRepository>()
    .AddScoped<IProductRepository, ProductRepository>();

builder.Services
    .AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services
    .AddSingleton<ProblemDetailsFactory, CustomProblemDetailsFactory>();

builder.Services.AddOpenApi();

builder.Services
    .AddControllers()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.AllowTrailingCommas = false;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
        options.JsonSerializerOptions.IncludeFields = false;
        options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Disallow;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
#endregion

var app = builder.Build();

#region HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options => options
        .WithDarkMode(true)
        .WithDarkModeToggle(true)
        .WithTitle("Catalog API Reference")
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
    );
}

app.UseAuthorization();
app.UseValidation();
app.MapControllers();
#endregion

app.Run();
