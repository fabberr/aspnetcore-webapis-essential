using System;
using System.Text.Json;
using Catalog.Api.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace Catalog.Api.Factories;

/// <summary>
/// A custom <see cref="ProblemDetailsFactory"/> implementation.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CustomProblemDetailsFactory"/> class.
/// </remarks>
/// <param name="logger">
/// A <see cref="ILogger{TCategoryName}"/> instance.
/// </param>
public sealed class CustomProblemDetailsFactory(
    ILogger<CustomProblemDetailsFactory> logger
)
    : ProblemDetailsFactory
{
    #region Constants
    private readonly static JsonSerializerOptions _stJsonSerializerOptions = new() {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
    };
    #endregion

    #region Dependencies
    private readonly ILogger<CustomProblemDetailsFactory> _logger = logger;
    #endregion

    #region ProblemDetailsFactory
    /// <inheritdoc/>
    public override ProblemDetails CreateProblemDetails(
        HttpContext httpContext,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null
    )
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        var context = new ProblemDetailsContext() {
            HttpContext = httpContext,
            ProblemDetails = new ProblemDetails(),
        };

        context.EnsureMembersInitializedOrSetToDefaultValues(statusCode, title, type, detail, instance);
        context.TryAddDefaultExtensions();

#if DEBUG
        _logger.LogDebug(
            message: Constants.Messages.Logging.Debug.ObjectCreated,
            DateTime.UtcNow, nameof(ProblemDetails),
            JsonSerializer.Serialize(context.ProblemDetails, _stJsonSerializerOptions)
        );
#endif

        return context.ProblemDetails;
    }

    /// <inheritdoc/>
    public override ValidationProblemDetails CreateValidationProblemDetails(
        HttpContext httpContext,
        ModelStateDictionary modelStateDictionary,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null
    )
    {
        ArgumentNullException.ThrowIfNull(httpContext);
        ArgumentNullException.ThrowIfNull(modelStateDictionary);

        var context = new ProblemDetailsContext() {
            HttpContext = httpContext,
            ProblemDetails = new ValidationProblemDetails(modelStateDictionary),
        };

        context.EnsureMembersInitializedOrSetToDefaultValues(statusCode, title, type, detail, instance);
        context.TryAddDefaultExtensions();

#if DEBUG
        _logger.LogDebug(
            message: Constants.Messages.Logging.Debug.ObjectCreated,
            DateTime.UtcNow, nameof(ValidationProblemDetails),
            JsonSerializer.Serialize(context.ProblemDetails, _stJsonSerializerOptions)
        );
#endif

        return (ValidationProblemDetails)context.ProblemDetails;
    }
    #endregion
}
