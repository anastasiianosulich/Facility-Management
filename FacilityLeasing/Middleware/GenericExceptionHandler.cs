using FacilityLeasing.Extensions;
using FacilityLeasing.Helpers;

namespace FacilityLeasing.Middleware;

public sealed class GenericExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GenericExceptionHandler> _logger;

    public GenericExceptionHandler(RequestDelegate next, ILogger<GenericExceptionHandler> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong: {ex}");
            await HandleException(context, ex);
        }
    }

    private static Task HandleException(HttpContext context, Exception exception)
    {
        var (errorDetails, statusCode) = exception.GetErrorDetailsAndStatusCode();

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        return context.Response.WriteAsync(SerializationHelper.SerializeJson(errorDetails));
    }
}