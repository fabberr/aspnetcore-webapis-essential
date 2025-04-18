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

var builder = WebApplication.CreateBuilder(args);

#region Configuration
var appSettings = builder.Configuration.ConfigureAppSettings();

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
        optionsAction: (optionsBuilder) => optionsBuilder
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
    .AddControllers()
    .AddJsonOptions((options) => {
        options.JsonSerializerOptions.AllowTrailingCommas = false;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
        options.JsonSerializerOptions.IncludeFields = false;
        options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Disallow;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services
    .AddSingleton<ProblemDetailsFactory, CustomProblemDetailsFactory>();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

builder.Services
    .Configure<RouteOptions>((options) => {
        options.LowercaseUrls = true;
        options.LowercaseQueryStrings = true;
    });
#endregion

var app = builder.Build();

#region HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseValidation();
app.MapControllers();
#endregion

app.Run();

// @todo: Improve problem details "Title" to return messages specific to the HTTP response status code
