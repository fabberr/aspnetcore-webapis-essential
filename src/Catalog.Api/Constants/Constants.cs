using Microsoft.AspNetCore.Http;

namespace Catalog.Api.Constants;

internal static class Messages
{
    internal static class Validation
    {
        internal const string InvalidValue = "The value '{0}' is not valid.";
        
        internal const string ResourceIdMismatch = "The resource id '{0}' does not match the entity id '{1}'.";
    } 
}

internal static class ProblemDetails
{
    internal static class Default
    {
        internal const int Status = StatusCodes.Status400BadRequest;

        internal const string Title = "An error occurred while processing your request.";
    }

    internal static class Extensions
    {
        internal const string RequestId = "requestId";
        internal const string TraceId = "traceId";
    }
}
