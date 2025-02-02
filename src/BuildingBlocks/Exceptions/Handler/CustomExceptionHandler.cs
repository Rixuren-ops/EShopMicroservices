using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Exceptions.Handler
{
    public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError("An exception occurred: {Exception}, Time of occurrence {time}", exception, DateTime.UtcNow);

            (string Detail, String Title, int StatusCode) details = exception switch
            {
                InternalServerException => (exception.Message, GetType().Name, StatusCodes.Status500InternalServerError),
                ValidationException => (exception.Message, GetType().Name, StatusCodes.Status400BadRequest),
                BadRequestException => (exception.Message, GetType().Name, StatusCodes.Status400BadRequest),
                NotFoundException => (exception.Message, GetType().Name, StatusCodes.Status404NotFound),
                _ => (exception.Message, GetType().Name, StatusCodes.Status500InternalServerError)
            };

            var problemDetails = new ProblemDetails
            {
                Title = details.Title,
                Detail = details.Detail,
                Status = details.StatusCode,
                Instance = context.Request.Path
            };

            problemDetails.Extensions.Add("traceId", context.TraceIdentifier);

            if(exception is ValidationException validationException)
            {
                problemDetails.Extensions.Add("ValidationErrors", validationException.Errors);
            }

            await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken :cancellationToken );
            return true;

        }
    }
}
