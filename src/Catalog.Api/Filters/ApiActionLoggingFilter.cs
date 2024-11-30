using System;
using System.Diagnostics;
using Catalog.Api.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Catalog.Api.Filters;

/// <summary>
/// Implements <see cref="IActionFilter"/>.<br/>
/// A filter for logging controller actions.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ApiActionLoggingFilter"/> class.
/// </remarks>
/// <param name="logger">
/// A named <see cref="ILogger{TCategoryName}"/> instance.
/// </param>
public class ApiActionLoggingFilter(ILogger<ApiActionLoggingFilter> logger) : IActionFilter
{
    #region Fields
    private readonly ILogger<ApiActionLoggingFilter> _logger = logger;
    private readonly long _startingTimestamp = Stopwatch.GetTimestamp();
    #endregion

    #region IActionFilter
    /// <inheritdoc/>
    public void OnActionExecuting(ActionExecutingContext context) => _logger.LogInformation(
        """
        [{Timestamp:dd/MM/yyyy hh:mm:ss.fffffff}] Executing Action: {ActionName}
        HTTPS Enabled: {IsHttps}
        Route: {Route}
        Model State: {ModelValidationState}
        """,
        DateTime.UtcNow,
        context.ActionDescriptor.AttributeRouteInfo?.Name ?? context.ActionDescriptor.DisplayName,
        context.HttpContext.Request.IsHttps,
        context.HttpContext.Request.GetFormattedRouteWithQuery(),
        context.ModelState.ValidationState
    );

    /// <inheritdoc/>
    public void OnActionExecuted(ActionExecutedContext context) => _logger.LogInformation(
        """
        [{Timestamp:dd/MM/yyyy hh:mm:ss.fffffff}] Executed Action: {ActionName}
        HTTPS Enabled: {IsHttps}
        Route: {Route}
        Response Status Code: {HttpResponseStatusCode}
        Elapsed Time: {ElapsedTime}
        """,
        DateTime.UtcNow,
        context.ActionDescriptor.AttributeRouteInfo?.Name ?? context.ActionDescriptor.DisplayName,
        context.HttpContext.Request.IsHttps,
        context.HttpContext.Request.GetFormattedRouteWithQuery(),
        context.HttpContext.Response.StatusCode,
        Stopwatch.GetElapsedTime(_startingTimestamp)
    );
    #endregion
}
