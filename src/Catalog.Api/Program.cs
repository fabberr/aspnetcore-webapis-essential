using System.Text.Json;
using System.Text.Json.Serialization;
using Catalog.Api.Extensions;
using Catalog.Api.Factories;
using Catalog.Core.Context;
using Catalog.Core.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

#region Configuration
var appSettings = builder.Configuration.ConfigureAppSettings();
#endregion

#region Services
builder.Services.AddDbContext<CatalogDbContext>(
    optionsAction: (optionsBuilder) => optionsBuilder
        .ConfigureDefaultDatabaseConnection(appSettings),
    contextLifetime: ServiceLifetime.Scoped,
    optionsLifetime: ServiceLifetime.Scoped
);

builder.Services.AddControllers()
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
