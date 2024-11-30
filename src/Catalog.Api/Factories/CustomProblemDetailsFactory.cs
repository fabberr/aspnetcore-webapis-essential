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
/// A <see cref="ProblemDetailsFactory"/> implementation providing custom behavior.
/// </summary>
public sealed class CustomProblemDetailsFactory(ILoggerFactory loggerFactory) : ProblemDetailsFactory
{
    #region Constants
    private readonly static JsonSerializerOptions _stJsonSerializerOptions = new() {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
    };
    #endregion

    #region Fields
    private readonly ILogger<CustomProblemDetailsFactory> _logger = loggerFactory.CreateLogger<CustomProblemDetailsFactory>();
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
        var serializedProblemDetails = JsonSerializer.Serialize(
            value: context.ProblemDetails,
            options: _stJsonSerializerOptions
        );
        _logger.LogDebug("Created ProblemDetails object (JSON): {serializedValue}", serializedProblemDetails);
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
        var serializedValidationProblemDetails = JsonSerializer.Serialize(
            value: (ValidationProblemDetails)context.ProblemDetails,
            options: _stJsonSerializerOptions
        );
        _logger.LogDebug("Created ValidationProblemDetails object (JSON): {serializedValue}", serializedValidationProblemDetails);
#endif

        return (ValidationProblemDetails)context.ProblemDetails;
    }
    #endregion
}
