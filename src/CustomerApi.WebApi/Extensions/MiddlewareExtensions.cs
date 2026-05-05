using CustomerApi.WebApi.Middlewares;

namespace CustomerApi.WebApi.Extensions;

internal static class MiddlewareExtensions
{
    public static void UseErrorHandling(this IApplicationBuilder builder) =>
        builder.UseMiddleware<ErrorHandlingMiddleware>();

}
