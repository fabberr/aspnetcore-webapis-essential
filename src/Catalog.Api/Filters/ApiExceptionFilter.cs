using System;
using System.Threading.Tasks;
using Catalog.Api.Constants;
using Catalog.Api.Extensions;
using Catalog.Core.Exceptions;
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
        var logLevel = LogLevel.Error;

        if (context.Exception is ModelValidationException validationException)
        {
            // Do not log instances of `ValidationException` as errors.
            logLevel = LogLevel.None;

            validationException.ModelState.CopyTo(context.ModelState);
        }

        _logger.Log(
            logLevel: logLevel,
            message: Messages.Logging.Error.UnhandledExceptionThrownWhileExecutingAction,
            DateTime.UtcNow, context.ActionDescriptor.AttributeRouteInfo?.Name ?? context.ActionDescriptor.DisplayName,
            context.HttpContext.Request.GetFormattedRouteWithQuery(),
            context.Exception
        );

        var problemDetails = context.Exception switch
        {
            ModelValidationException => _problemDetailsFactory.CreateValidationProblemDetails(
                httpContext: context.HttpContext,
                modelStateDictionary: context.ModelState,
                statusCode: StatusCodes.Status400BadRequest
            ),

            _ => _problemDetailsFactory.CreateProblemDetails(
                httpContext: context.HttpContext,
                statusCode: StatusCodes.Status500InternalServerError
            )
        };

        context.Result = new ObjectResult(problemDetails);

        return Task.CompletedTask;
    }
    #endregion
}
