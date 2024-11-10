using Catalog.Api.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Catalog.Api.Extensions;

internal static class MiddlewareExtensions
{
    internal static IApplicationBuilder UseValidation(this IApplicationBuilder builder)
        => builder.UseMiddleware<ValidationMiddleware>();
}
