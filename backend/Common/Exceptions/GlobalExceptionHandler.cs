using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace backend_app.Common.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, $"Wystąpił nieobsłużony wyjątek: {exception.Message}");

            var (statusCode, title, details, errors) = exception switch
            {
                ValidationException validationException => (
                StatusCodes.Status400BadRequest,
                "Validation Error",
                "Wystąpiły błedy walidacji.",
                validationException.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())),

                InvalidOperationException invalidOperationException => (
                    StatusCodes.Status400BadRequest,
                    "Invalid Operation",
                    exception.Message,
                    null),

                _ => (
                    StatusCodes.Status500InternalServerError,
                    "Internal Server Error",
                    "Wystąpił nieoczekiwany błąd po stronie serwera.",
                    null)
            };

            httpContext.Response.StatusCode = statusCode;

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = details,
                Instance = httpContext.Request.Path
            };

            if(errors != null)
            {
                problemDetails.Extensions.Add("errors", errors);
            }

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
