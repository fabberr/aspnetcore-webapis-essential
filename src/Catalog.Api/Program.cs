using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Catalog.Api.Extensions;
using Catalog.Api.Factories;
using Catalog.Core.Abstractions.Repositories;
using Catalog.Core.Abstractions.Repositories.Interfaces;
using Catalog.Core.Attributes;
using Catalog.Core.Context;
using Catalog.Core.Extensions;
using Catalog.Core.Models.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
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
builder.Services.AddDbContext<CatalogDbContext>(
    optionsAction: (optionsBuilder) => optionsBuilder
        .ConfigureDefaultDatabaseConnection(appSettings),
    contextLifetime: ServiceLifetime.Scoped,
    optionsLifetime: ServiceLifetime.Scoped
);

builder.Services
    .AddScoped<ICategoryRepository, EntityFrameworkCoreCategoryRepository>();

builder.Services
    .AddControllers()
    .AddJsonOptions((options) => {
        options.JsonSerializerOptions.AllowTrailingCommas = false;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddTransient<ProblemDetailsFactory, CustomProblemDetailsFactory>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<RouteOptions>((options) => {
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
