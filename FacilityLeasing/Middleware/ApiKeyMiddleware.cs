using FacilityLeasing.Exceptions;
using Microsoft.Extensions.Options;
using FacilityLeasing.Options;

namespace FacilityLeasing.Middleware;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ApiKeySettings _apiKeySettings;

    public ApiKeyMiddleware(RequestDelegate next, IOptions<ApiKeySettings> apiKeySettings)
    {
        _next = next;
        _apiKeySettings = apiKeySettings.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.ContainsKey(Constants.ApiKeyHeaderName) 
            || context.Request.Headers[Constants.ApiKeyHeaderName] != _apiKeySettings.ApiKey)
        {
            throw new AuthorizationException();
        }

        await _next(context);
    }
}
