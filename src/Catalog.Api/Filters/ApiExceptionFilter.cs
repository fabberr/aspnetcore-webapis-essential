using System;
using System.Threading.Tasks;
using Catalog.Api.Constants;
using Catalog.Api.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Catalog.Api.Filters;

/// <summary>
/// Provides a global unhandled exception handler for API Controller actions.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ApiExceptionFilter"/> class.
/// </remarks>
/// <param name="logger">
/// A <see cref="ILogger{TCategoryName}"/> instance.
/// </param>
/// <param name="problemDetailsFactory">
/// A <see cref="ProblemDetailsFactory"/> instance.
/// </param>
public sealed class ApiExceptionFilter(
    ILogger<ApiExceptionFilter> logger,
    ProblemDetailsFactory problemDetailsFactory
)
    : IAsyncExceptionFilter
{
    #region Dependencies
    private readonly ILogger<ApiExceptionFilter> _logger = logger;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;
    #endregion

    #region IAsyncExceptionFilter
    /// <inheritdoc/>
    public Task OnExceptionAsync(ExceptionContext context)
    {
        var actionName = context.ActionDescriptor.AttributeRouteInfo?.Name ?? context.ActionDescriptor.DisplayName;
        _logger.LogError(
            message: Messages.Logging.Error.UnhandledExceptionThrownWhileExecutingAction,
            DateTime.UtcNow, actionName,
            context.HttpContext.Request.GetFormattedRouteWithQuery(),
            context.Exception
        );

        context.Result = new ObjectResult(
            _problemDetailsFactory.CreateProblemDetails(
                httpContext: context.HttpContext,
                statusCode: StatusCodes.Status500InternalServerError
            )
        );

        return Task.CompletedTask;
    }
    #endregion
}
