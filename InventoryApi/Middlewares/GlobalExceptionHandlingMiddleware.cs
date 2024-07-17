using InventoryApi.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InventoryApi.Middlewares
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static IActionResult CreateActionResult(HttpContext context, HttpStatusCode status, string title, string message)
        {
            var problemDetails = new ProblemDetails
            {
                Status = (int)status,
                Title = title,
                Detail = message,
                Instance = context.Request.Path
            };

            return status switch
            {
                HttpStatusCode.BadRequest => new BadRequestObjectResult(problemDetails),
                HttpStatusCode.NotFound => new NotFoundObjectResult(problemDetails),
                HttpStatusCode.Unauthorized => new UnauthorizedObjectResult(problemDetails),
                HttpStatusCode.Forbidden => new ObjectResult(problemDetails) { StatusCode = (int)HttpStatusCode.Forbidden },
                HttpStatusCode.Conflict => new ConflictObjectResult(problemDetails),
                HttpStatusCode.RequestTimeout => new ObjectResult(problemDetails) { StatusCode = (int)HttpStatusCode.RequestTimeout },
                _ => new ObjectResult(problemDetails) { StatusCode = (int)HttpStatusCode.InternalServerError }
            };
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode status;
            string message;
            string title;

            switch (exception)
            {
                case BadRequestException badRequestException:
                    message = badRequestException.Message;
                    status = HttpStatusCode.BadRequest;
                    title = "Bad Request Exception Occurred";
                    break;
                case NotFoundException notFoundException:
                    message = notFoundException.Message;
                    status = HttpStatusCode.NotFound;
                    title = "Not Found Exception Occurred";
                    break;
                case UnauthorizedException unauthorizedException:
                    message = unauthorizedException.Message;
                    status = HttpStatusCode.Unauthorized;
                    title = "Unauthorized Exception Occurred";
                    break;
                case ForbiddenAccessException forbiddenException:
                    message = forbiddenException.Message;
                    status = HttpStatusCode.Forbidden;
                    title = "Forbidden Access Exception Occurred";
                    break;
                case ValidationException validationException:
                    message = validationException.Message;
                    status = HttpStatusCode.Conflict;
                    title = "Validation Exception Occurred";
                    break;
                case TimeoutException timeoutException:
                    message = timeoutException.Message;
                    status = HttpStatusCode.RequestTimeout;
                    title = "Timeout Exception Occurred";
                    break;
                // Add other exception types as needed
                default:
                    status = HttpStatusCode.InternalServerError;
                    title = "Internal Server Error Occurred";
                    message = "An error occurred while processing your request.";
                    break;
            }

            var problemDetails = new ProblemDetails
            {
                Status = (int)status,
                Title = title,
                Detail = message,
                Instance = context.Request.Path
            };

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)status;

            var jsonResult = System.Text.Json.JsonSerializer.Serialize(problemDetails);
            await context.Response.WriteAsync(jsonResult);
        }
    }
}
