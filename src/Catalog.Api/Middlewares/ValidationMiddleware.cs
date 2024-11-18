using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Constants;
using Catalog.Api.Extensions;
using Catalog.Core.Models.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace Catalog.Api.Middlewares;

internal readonly record struct AllowedRange(uint Min, uint Max)
{
    public static implicit operator (uint Min, uint Max)(AllowedRange value) => (value.Min, value.Max);

    public static implicit operator AllowedRange((uint Min, uint Max) value) => new(value.Min, value.Max);
}

public class ValidationMiddleware(RequestDelegate next, ProblemDetailsFactory problemDetailsFactory)
{
    private readonly RequestDelegate _next = next;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    public async Task InvokeAsync(HttpContext httpContext, IOptionsSnapshot<ApiBehaviorSettings> options)
    {
        var _allowedIntegerParameterValueRanges = new Dictionary<string, AllowedRange>() {
            ["limit"]  = (Min: 1u, Max: options.Value.MaxItemsPerPage),
            ["offset"] = (Min: 0u, Max: int.MaxValue),
        }.AsReadOnly();

        var modelStateDictionary = new ModelStateDictionary();

        foreach (var (key, (min, max)) in _allowedIntegerParameterValueRanges
            .Where((kvp) => httpContext.Request.Query.ContainsKey(kvp.Key)))
        {
            bool parsedSuccessfully = httpContext.Request.Query.TryParseValue<uint>(key, out var value, out var invalidValue);

            if (!parsedSuccessfully || !(value >= min && value <= max))
            {
                string errorMessage = string.Format(Messages.Validation.InvalidValue, invalidValue ?? string.Empty);
                modelStateDictionary.TryAddModelError(key, errorMessage);
            }
        }

        if (!modelStateDictionary.IsValid)
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            await httpContext.Response.WriteAsJsonAsync(
                _problemDetailsFactory.CreateValidationProblemDetails(
                    httpContext,
                    modelStateDictionary,
                    StatusCodes.Status400BadRequest
                )
            );

            return;
        }

        await _next(httpContext);
    }
}
