using System;
using Microsoft.AspNetCore.Http;

namespace Catalog.Api.Extensions;

/// <summary>
/// Provides extension methods for <see cref="HttpRequest"/> instances.
/// </summary>
internal static class HttpRequestExtensions
{
    /// <summary>
    /// Returns a string representation of the route for the requested resource.
    /// </summary>
    /// <remarks>
    /// The format is as follows:<br/>
    /// <br/>
    /// <code>
    /// (&lt;HTTP version&gt;) &lt;HTTP method&gt; &lt;resource path&gt;[&lt;escaped query string&gt;]
    /// </code>
    /// </remarks>
    /// <param name="request"></param>
    /// <returns>
    /// A string representation of the route for the requested resource.
    /// </returns>
    internal static string GetFormattedRouteWithQuery(this HttpRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        string queryString = request.QueryString.HasValue
            ? request.QueryString.Value
            : string.Empty;

        return $"({request.Protocol}) {request.Method} {request.Path}{queryString}";
    }
}
