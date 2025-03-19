using System.Net;
using FoxStevenle.API.Types.OptionalResult;
using Newtonsoft.Json;

namespace FoxStevenle.API.Middleware;

public class ExceptionLoggingMiddleware(RequestDelegate next, ILogger<ExceptionLoggingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occurred while processing the request.");

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            var response = new OptionalError
            {
#if DEBUG
                Message = ex.Message,
#else
                Message = "An internal server error occurred",
#endif
                Type = OptionalErrorType.InternalServerError
            };
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}