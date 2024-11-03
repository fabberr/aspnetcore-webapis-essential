using Catalog.Core.Context;
using Catalog.Core.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<RouteOptions>(options => {
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
app.MapControllers();
#endregion

app.Run();
