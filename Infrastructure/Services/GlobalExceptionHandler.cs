using System.Net;
using System.Security.Authentication;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger){
        _logger = logger;
    }
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var result = new ProblemDetails();
        switch (exception)
        {
            case ArgumentException argumentException:
                result = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Type = argumentException.GetType().Name,
                    Title = "An unexpected error occurred",
                    Detail = argumentException.Message,
                    Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                };
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                _logger.LogError(argumentException, $"Exception occured : {argumentException.Message}");
                break;

            case InvalidCredentialException invalidCredentialException:
                result = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.Unauthorized,
                    Type = invalidCredentialException.GetType().Name,
                    Title = "An unexpected error occurred",
                    Detail = invalidCredentialException.Message,
                    Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                };
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                _logger.LogError(invalidCredentialException, $"Exception occured : {invalidCredentialException.Message}");
                break;

            default:
                result = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = exception.GetType().Name,
                    Title = "An unexpected error occurred",
                    Detail = exception.Message,
                    Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
                };
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                _logger.LogError(exception, $"Exception occured : {exception.Message}");
                break;
        }
        
        await httpContext.Response.WriteAsJsonAsync(result, cancellationToken: cancellationToken);
        return true;
    }
}