using System;
using Catalog.Api.Models.Settings;
using Catalog.Core.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

#region Configuration
var appSettings = builder.Configuration.Get<AppSettings>(
    (binderOptions) => binderOptions.BindNonPublicProperties = true
)!;
#endregion

#region DB Context
var defaultDatabaseConnection = builder.Configuration.GetConnectionString("DefaultDatabaseConnection");

builder.Services.AddDbContext<CatalogDbContext>(
    (options) => {
        switch (appSettings.DefaultDatabaseProvider)
        {
            case DatabaseProvider.Sqlite3:
            options.UseSqlite(connectionString: defaultDatabaseConnection);
                break;
            default:
                throw new NotSupportedException("Invalid Database Provider.");
        }
    }
);
#endregion

#region Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion

var app = builder.Build();

#region HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
#endregion

app.Run();
