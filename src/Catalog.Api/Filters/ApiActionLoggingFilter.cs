using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Catalog.Api.Constants;
using Catalog.Api.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Catalog.Api.Filters;

/// <summary>
/// Provides a global API Controller action logging filter.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ApiActionLoggingFilter"/> class.
/// </remarks>
/// <param name="logger">
/// A <see cref="ILogger{TCategoryName}"/> instance.
/// </param>
public class ApiActionLoggingFilter(ILogger<ApiActionLoggingFilter> logger): IAsyncActionFilter
{

    #region Dependencies
    private readonly ILogger<ApiActionLoggingFilter> _logger = logger;
    #endregion

    #region Fields
    private readonly long _startingTimestamp = Stopwatch.GetTimestamp();
    #endregion

    #region IAsyncActionFilter
    /// <inheritdoc/>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var actionName = context.ActionDescriptor.AttributeRouteInfo?.Name ?? context.ActionDescriptor.DisplayName;
        var route = context.HttpContext.Request.GetFormattedRouteWithQuery();

        _logger.LogInformation(
            message: Messages.Logging.Information.ActionStartedExecuting,
            DateTime.UtcNow, actionName,
            route,
            context.ModelState.ValidationState
        );
        
        await next(); // Invoke the next Action Filter in the pipeline or the Action itself.

        _logger.LogInformation(
            message: Messages.Logging.Information.ActionFinishedExecuting,
            DateTime.UtcNow, actionName,
            route,
            context.HttpContext.Response.StatusCode,
            Stopwatch.GetElapsedTime(_startingTimestamp)
        );
    }
    #endregion
}
