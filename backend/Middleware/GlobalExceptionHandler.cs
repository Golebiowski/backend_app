using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace backend_app.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var problemDetails = new ProblemDetails
            {
                Instance = httpContext.Request.Path
            };

            switch (exception)
            {
                case ValidationException fluentException:
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Błąd walidacji";
                    problemDetails.Detail = "Jedno lub więcej pól nie przeszło walidacji";
                    problemDetails.Extensions["errors"] = fluentException.Errors    
                        .Select(e => new { e.PropertyName, e.ErrorMessage });
                    break;

                case InvalidOperationException domainException:
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Błąd reguły biznesowej";
                    problemDetails.Detail = domainException.Message;
                    break;

                default:
                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                    problemDetails.Title = "Wewnętrzny błąd serwera";
                    problemDetails.Detail = "Wystąpił nieoczekiwany błąd. Proszę spróbować ponownie później.";
                    break;
            }

            httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
