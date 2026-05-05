using System.Net.Mime;
using CustomerApi.Core.Extensions;
using CustomerApi.Domain.Exceptions;
using CustomerApi.WebApi.Models;

namespace CustomerApi.WebApi.Middlewares;

public class ErrorHandlingMiddleware(
    RequestDelegate next,
    ILogger<ErrorHandlingMiddleware> logger,
    IHostEnvironment environment
    )
{
    private const string ErrorMessage = "Ocorreu um erro interno ao processar sua solicitação.";

    private static readonly string ApiResponseJson = ApiResponse.InternalServerError(ErrorMessage).ToJson();

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (DomainException ex)
        {
           
            httpContext.Response.StatusCode = 400; 
            await httpContext.Response.WriteAsync(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Uma exceção inesperada foi lançada: {Message}", ex.Message);

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            if (environment.IsDevelopment())
            {
                httpContext.Response.ContentType = MediaTypeNames.Text.Plain;
                await httpContext.Response.WriteAsync(ex.ToString());
            }
            else
            {
                httpContext.Response.ContentType = MediaTypeNames.Application.Json;
                await httpContext.Response.WriteAsync(ApiResponseJson);
            }

        }
    }
}
