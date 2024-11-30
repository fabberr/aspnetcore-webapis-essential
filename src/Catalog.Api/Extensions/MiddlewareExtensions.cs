using System;
using Catalog.Api.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Catalog.Api.Extensions;

/// <summary>
/// Provides extension methods for registering custom middlewares to the
/// application's request pipeline.
/// </summary>
internal static class CustomMiddlewareExtensions
{
    /// <summary>
    /// Adds the <see cref="ValidationMiddleware"/> to the specified
    /// <see cref="IApplicationBuilder"/>, which enables custom request
    /// validation logic.
    /// </summary>
    /// <param name="app">
    /// The <see cref="IApplicationBuilder"/> instance.
    /// </param>
    /// <returns>
    /// The <see cref="IApplicationBuilder"/> instance.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="app"/> is <see langword="null"/>.
    /// </exception>
    internal static IApplicationBuilder UseValidation(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        return app.UseMiddleware<ValidationMiddleware>();
    }
}
