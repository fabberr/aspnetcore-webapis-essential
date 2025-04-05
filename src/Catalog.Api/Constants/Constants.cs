namespace Catalog.Api.Constants;

/// <summary>
/// Defines application constants relating to general error/logging messages.
/// </summary>
internal static class Messages
{
    /// <summary>
    /// Defines general validation error messages.
    /// </summary>
    internal static class Validation
    {
        /// <summary>
        /// The provided value was invalid.
        /// </summary>
        internal const string InvalidValue = "The value '{0}' is not valid.";
        
        /// <summary>
        /// The specified resouce key does not match the entity key.
        /// </summary>
        internal const string SpecifiedKeyDoesNotMatchEntityKey = "The specified key '{0}' does not match the entity key '{1}'.";
    }

    /// <summary>
    /// Defines gerenal logging message formats.
    /// </summary>
    internal static class Logging
    {
        /// <summary>
        /// Defines Information message formats.
        /// </summary>
        internal static class Information
        {
            /// <summary>
            /// An Action has started executing.
            /// </summary>
            internal const string ActionStartedExecuting = """
                [{Timestamp:yyyy.MM.dd hh:mm:ss.fffffff}] Executing Action: {ActionName}
                Route: {Route}
                Model State: {ModelValidationState}
                """;

            /// <summary>
            /// An Action has finished executing.
            /// </summary>
            internal const string ActionFinishedExecuting = """
                [{Timestamp:yyyy.MM.dd hh:mm:ss.fffffff}] Executed Action: {ActionName}
                Route: {Route}
                Response Status Code: {HttpResponseStatusCode}
                Elapsed Time: {ElapsedTime}
                """;
        }

        /// <summary>
        /// Defines Error message formats.
        /// </summary>
        internal static class Error
        {
            /// <summary>
            /// An unhandled exception was thrown while executing an Action.
            /// </summary>
            internal const string UnhandledExceptionThrownWhileExecutingAction = """
                [{Timestamp:yyyy.MM.dd hh:mm:ss.fffffff}] An unhandled exception was thrown while executing Action: {ActionName}
                Route: {Route}
                Exception: {Exception}
                """;
        }

        /// <summary>
        /// Defines Debug message formats.
        /// </summary>
        internal static class Debug
        {
            /// <summary>
            /// Created a new instance of an object.
            /// </summary>
            internal const string ObjectCreated = """
                [{Timestamp:yyyy.MM.dd hh:mm:ss.fffffff}] Created {TypeName} object
                Object: {SerializedObject}
                """;
        }
    }
}

/// <summary>
/// Defines application constants relating to the
/// <see href="https://datatracker.ietf.org/doc/html/rfc9457">
///     [RFC 9457] Problem Details for HTTP APIs
/// </see>
/// specification.
/// </summary>
internal static class ProblemDetails
{
    /// <summary>
    /// Defines default values for <see cref="Microsoft.AspNetCore.Mvc.ProblemDetails"/> properties.
    /// </summary>
    internal static class DefaultValues
    {
        /// <summary>
        /// The default value for the Status property of a <see cref="Microsoft.AspNetCore.Mvc.ProblemDetails"/> instance.
        /// </summary>
        internal const int Status = Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError;

        /// <summary>
        /// The default value for the Title property of a <see cref="Microsoft.AspNetCore.Mvc.ProblemDetails"/> instance.
        /// </summary>
        internal const string Title = "An error occurred while processing your request.";
    }

    /// <summary>
    /// Defines the names of <see cref="Microsoft.AspNetCore.Mvc.ProblemDetails"/> extensions.
    /// </summary>
    internal static class ExtensionNames
    {
        /// <summary>
        /// The name of the RequestId extension.
        /// </summary>
        internal const string RequestId = "requestId";

        /// <summary>
        /// The name of the TraceId extension.
        /// </summary>
        internal const string TraceId = "traceId";
    }
}
