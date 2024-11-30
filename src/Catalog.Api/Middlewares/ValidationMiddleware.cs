using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Constants;
using Catalog.Api.Extensions;
using Catalog.Core.Models.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace Catalog.Api.Middlewares;

/// <summary>
///     Represents a valid range for a <see cref="uint"/> value.
/// </summary>
/// <param name="Min">
///     The minimum allowed value.
/// </param>
/// <param name="Max">
///     The maximum allowed value.
/// </param>
internal readonly record struct UInt32AllowedRange(uint Min, uint Max)
{
    /// <summary>
    ///     Implicit conversion operator from <see cref="UInt32AllowedRange"/>
    ///     to <see cref="(uint Min, uint Max)"/>.
    /// </summary>
    /// <param name="value">
    ///     The <see cref="UInt32AllowedRange"/> instance.
    /// </param>
    public static implicit operator (uint Min, uint Max)(UInt32AllowedRange value) => (value.Min, value.Max);

    /// <summary>
    ///     Implicit conversion operator from <see cref="(uint Min, uint Max)"/>
    ///     to <see cref="UInt32AllowedRange"/>.
    /// </summary>
    /// <param name="value">
    ///     The <see cref="(uint Min, uint Max)"/> instance.
    /// </param>
    public static implicit operator UInt32AllowedRange((uint Min, uint Max) value) => new(value.Min, value.Max);
}

/// <summary>
/// Provides custom request validation logic.
/// </summary>
public class ValidationMiddleware(RequestDelegate next, ProblemDetailsFactory problemDetailsFactory)
{
    #region Fields
    private readonly RequestDelegate _next = next;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;
    #endregion

    #region Middleware Action
    public async Task InvokeAsync(HttpContext httpContext, IOptionsSnapshot<ApiBehaviorSettings> options)
    {
        var _allowedIntegerParameterValueRanges = new Dictionary<string, UInt32AllowedRange>() {
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
    #endregion
}
