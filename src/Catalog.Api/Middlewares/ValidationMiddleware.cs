using System.Threading.Tasks;
using Catalog.Api.Constants;
using Catalog.Api.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Catalog.Api.Middlewares;

public class ValidationMiddleware(RequestDelegate next, ProblemDetailsFactory problemDetailsFactory)
{
    private const uint LimitQueryParameterMaxValue = 100u;
    private const uint OffsetQueryParameterMaxValue = int.MaxValue;

    private readonly RequestDelegate _next = next;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    public async Task InvokeAsync(HttpContext httpContext)
    {
        var modelStateDictionary = new ModelStateDictionary();

        bool hasLimit = httpContext.Request.Query.TryParseValue<uint>("limit", out var limit);
        bool hasOffset = httpContext.Request.Query.TryParseValue<uint>("offset", out var offset);

        if (hasLimit && limit is 0 or > LimitQueryParameterMaxValue)
        {
            modelStateDictionary.AddModelError(nameof(limit), string.Format(Messages.Validation.InvalidValue, limit));
        }

        if (hasOffset && offset is < 0 or > OffsetQueryParameterMaxValue)
        {
            modelStateDictionary.AddModelError(nameof(offset), string.Format(Messages.Validation.InvalidValue, offset));
        }

        if (modelStateDictionary.ErrorCount > 0)
        {
            var validationProblemDetails = _problemDetailsFactory.CreateValidationProblemDetails(
                httpContext: httpContext,
                modelStateDictionary: modelStateDictionary,
                statusCode: StatusCodes.Status400BadRequest
            );

            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            await httpContext.Response.WriteAsJsonAsync(validationProblemDetails);

            return;
        }

        await _next(httpContext);
    }
}
