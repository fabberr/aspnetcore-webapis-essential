using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Extensions;

/// <summary>
/// Provides extension methods for <see cref="ProblemDetailsContext"/> instances.
/// </summary>
internal static class ProblemDetailsContextExtensions
{
    #region Constants
    /// <summary>
    /// Maps HTTP 4xx (Client Error) or 5xx (Server Error) status codes to
    /// their respectrive formal specification as defined in
    /// 
    /// <see href="https://datatracker.ietf.org/doc/html/rfc9110#section-15.5">
    ///     [RFC 9010], Section 15.5
    /// </see>
    /// 
    /// or
    /// 
    /// <see href="https://datatracker.ietf.org/doc/html/rfc9110#section-15.6">
    ///     [RFC 9010], Section 15.6
    /// </see>.<br/>
    /// <br/>
    /// ... or to pictures of cute dogs for fun and profit.
    /// </summary>
    private readonly static ReadOnlyDictionary<int, Uri> HttpErrorSpecificationUris = new Dictionary<int, Uri>() {
    #if DEBUG
        #region HTTP Client Error 4xx
        [StatusCodes.Status400BadRequest]                  = new Uri("https://http.dog/400"),
        [StatusCodes.Status401Unauthorized]                = new Uri("https://http.dog/401"),
        [StatusCodes.Status402PaymentRequired]             = new Uri("https://http.dog/402"),
        [StatusCodes.Status403Forbidden]                   = new Uri("https://http.dog/403"),
        [StatusCodes.Status404NotFound]                    = new Uri("https://http.dog/404"),
        [StatusCodes.Status405MethodNotAllowed]            = new Uri("https://http.dog/405"),
        [StatusCodes.Status406NotAcceptable]               = new Uri("https://http.dog/406"),
        [StatusCodes.Status407ProxyAuthenticationRequired] = new Uri("https://http.dog/407"),
        [StatusCodes.Status408RequestTimeout]              = new Uri("https://http.dog/408"),
        [StatusCodes.Status409Conflict]                    = new Uri("https://http.dog/409"),
        [StatusCodes.Status410Gone]                        = new Uri("https://http.dog/410"),
        [StatusCodes.Status411LengthRequired]              = new Uri("https://http.dog/411"),
        [StatusCodes.Status412PreconditionFailed]          = new Uri("https://http.dog/412"),
        [StatusCodes.Status413PayloadTooLarge]             = new Uri("https://http.dog/413"),
        [StatusCodes.Status414RequestUriTooLong]           = new Uri("https://http.dog/414"),
        [StatusCodes.Status415UnsupportedMediaType]        = new Uri("https://http.dog/415"),
        [StatusCodes.Status416RangeNotSatisfiable]         = new Uri("https://http.dog/416"),
        [StatusCodes.Status417ExpectationFailed]           = new Uri("https://http.dog/417"),
        [StatusCodes.Status418ImATeapot]                   = new Uri("https://http.dog/418"),
        [StatusCodes.Status421MisdirectedRequest]          = new Uri("https://http.dog/421"),
        [StatusCodes.Status422UnprocessableEntity]         = new Uri("https://http.dog/422"),
        [StatusCodes.Status426UpgradeRequired]             = new Uri("https://http.dog/426"),
        #endregion

        #region HTTP Server Error 5xx
        [StatusCodes.Status500InternalServerError]     = new Uri("https://http.dog/500"),
        [StatusCodes.Status501NotImplemented]          = new Uri("https://http.dog/501"),
        [StatusCodes.Status502BadGateway]              = new Uri("https://http.dog/502"),
        [StatusCodes.Status503ServiceUnavailable]      = new Uri("https://http.dog/503"),
        [StatusCodes.Status504GatewayTimeout]          = new Uri("https://http.dog/504"),
        [StatusCodes.Status505HttpVersionNotsupported] = new Uri("https://http.dog/505"),
        #endregion
    #else
        #region HTTP Client Error 4xx
        [StatusCodes.Status400BadRequest]                   = new Uri("https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1"),
        [StatusCodes.Status401Unauthorized]                 = new Uri("https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.2"),
        [StatusCodes.Status402PaymentRequired]              = new Uri("https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.3"),
        [StatusCodes.Status403Forbidden]                    = new Uri("https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.4"),
        [StatusCodes.Status404NotFound]                     = new Uri("https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5"),
        [StatusCodes.Status405MethodNotAllowed]             = new Uri("https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.6"),
        [StatusCodes.Status406NotAcceptable]                = new Uri("https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.7"),
        [StatusCodes.Status407ProxyAuthenticationRequired]  = new Uri("https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.8"),
        [StatusCodes.Status408RequestTimeout]               = new Uri("https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.9"),
        [StatusCodes.Status409Conflict]                     = new Uri("https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.10"),
        [StatusCodes.Status410Gone]                         = new Uri("https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.11"),
        [StatusCodes.Status411LengthRequired]               = new Uri("https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.12"),
        [StatusCodes.Status412PreconditionFailed]           = new Uri("https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.13"),
        [StatusCodes.Status413PayloadTooLarge]              = new Uri("https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.14"),
        [StatusCodes.Status414RequestUriTooLong]            = new Uri("https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.15"),
        [StatusCodes.Status415UnsupportedMediaType]         = new Uri("https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.16"),
        [StatusCodes.Status416RangeNotSatisfiable]          = new Uri("https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.17"),
        [StatusCodes.Status417ExpectationFailed]            = new Uri("https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.18"),
        [StatusCodes.Status418ImATeapot]                    = new Uri("https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.19"),
        [StatusCodes.Status421MisdirectedRequest]           = new Uri("https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.20"),
        [StatusCodes.Status422UnprocessableEntity]          = new Uri("https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.21"),
        [StatusCodes.Status426UpgradeRequired]              = new Uri("https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.22"),
        #endregion

        #region HTTP Server Error 5xx
        [StatusCodes.Status500InternalServerError]     = new Uri("https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.1"),
        [StatusCodes.Status501NotImplemented]          = new Uri("https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.2"),
        [StatusCodes.Status502BadGateway]              = new Uri("https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.3"),
        [StatusCodes.Status503ServiceUnavailable]      = new Uri("https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.4"),
        [StatusCodes.Status504GatewayTimeout]          = new Uri("https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.5"),
        [StatusCodes.Status505HttpVersionNotsupported] = new Uri("https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.6"),
        #endregion
    #endif
    }.AsReadOnly();
    #endregion

    /// <summary>
    ///     Sets members of the <see cref="ProblemDetails"/> instance held by this
    ///     context to the values provided in the arguments, if the instance
    ///     members are already not initialized.
    /// </summary>
    /// <remarks>
    ///     If neither a <see cref="ProblemDetails"/> member not its corresponding
    ///     argument have values, its value will be set as follows:
    ///     <list type="bullet">
    ///         <item>
    ///             <title><see cref="ProblemDetails.Status"/></title>
    ///             <description>
    ///                 The default value defined in the constant <see cref="Constants.ProblemDetails.DefaultValues.Status"/>.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <title><see cref="ProblemDetails.Title"/></title>
    ///             <description>
    ///                 The default value defined in the constant <see cref="Constants.ProblemDetails.DefaultValues.Title"/>.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <title><see cref="ProblemDetails.Type"/></title>
    ///             <description>
    ///                 An URI that points to a <see href="https://datatracker.ietf.org/doc/html/rfc9110#section-15.5">
    ///                 RFC 9110, Section 15.5</see> HTTP 4xx status code
    ///                 (Client Error) specification.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <title><see cref="ProblemDetails.Detail"/></title>
    ///             <description>
    ///                 No value set (<see langword="null"/>).
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <title><see cref="ProblemDetails.Instance"/></title>
    ///             <description>
    ///                 The HTTP method followed by the path section of the request URI, obtained from
    ///                 the <see cref="ProblemDetailsContext.HttpContext"/> instance.
    ///             </description>
    ///         </item>
    ///     </list>
    /// </remarks>
    /// <param name="context">
    ///     The <see cref="ProblemDetailsContext"/> instance.
    /// </param>
    /// <param name="statusCode">
    ///     <inheritdoc cref="ProblemDetails.Status" path="/summary"/>
    /// </param>
    /// <param name="title">
    ///     <inheritdoc cref="ProblemDetails.Title" path="/summary"/>
    /// </param>
    /// <param name="type">
    ///     <inheritdoc cref="ProblemDetails.Type" path="/summary"/>
    /// </param>
    /// <param name="detail">
    ///     <inheritdoc cref="ProblemDetails.Detail" path="/summary"/>
    /// </param>
    /// <param name="instance">
    ///     <inheritdoc cref="ProblemDetails.Instance" path="/summary"/>
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="context"/> is <see langword="null"/>.
    /// </exception>
    internal static void EnsureMembersInitializedOrSetToDefaultValues(
        this ProblemDetailsContext context,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null
    )
    {
        ArgumentNullException.ThrowIfNull(context);

        context.ProblemDetails.Status ??= statusCode ?? Constants.ProblemDetails.DefaultValues.Status;

        context.ProblemDetails.Title ??= title ?? Constants.ProblemDetails.DefaultValues.Title;

        context.ProblemDetails.Type ??= type ?? (
            HttpErrorSpecificationUris.TryGetValue(context.ProblemDetails.Status.Value, out var httpClientErrorSpecUri)
                ? httpClientErrorSpecUri
                : HttpErrorSpecificationUris[StatusCodes.Status400BadRequest]
        ).ToString();

        context.ProblemDetails.Detail ??= detail;
 
        context.ProblemDetails.Instance ??= instance ?? $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
    }

    /// <summary>
    ///     Adds default <see cref="ProblemDetails.Extensions"/> to the
    ///     <see cref="ProblemDetails"/> instance held by this context.
    /// </summary>
    /// <remarks>
    ///     The following extensions are set, if possible:
    ///     <list type="bullet">
    ///         <item>
    ///             <title><c>requestId</c></title>
    ///             <description>
    ///                 A unique identifier specific to the <see cref="HttpContext.Request"/> held by
    ///                 this context, obtained from the <see cref="HttpRequest.TraceIdentifier"/>.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <title><c>traceId</c></title>
    ///             <description>
    ///                 A unique identifier specific to the <see cref="HttpContext.Request"/> held by
    ///                 this context, obtained from the <see cref="IHttpActivityFeature"/> feature
    ///                 associated with the request.<br/>
    ///                 If the Activity feature cannot be retrieved, the extension is not set.
    ///             </description>
    ///         </item>
    ///     </list>
    /// </remarks>
    /// <param name="context">
    ///     The <see cref="ProblemDetailsContext"/> instance.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="context"/> is <see langword="null"/>.
    /// </exception>
    internal static void TryAddDefaultExtensions(this ProblemDetailsContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        context.ProblemDetails.Extensions.TryAdd(
            Constants.ProblemDetails.ExtensionNames.RequestId,
            context.HttpContext.TraceIdentifier
        );

        context.ProblemDetails.Extensions.TryAdd(
            Constants.ProblemDetails.ExtensionNames.TraceId,
            context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity?.Id
        );
    }
}
