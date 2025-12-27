using InventoryApi.DataContext;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Net;

namespace InventoryApi.Middlewares
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
        private readonly ApplicationDbContext _dbContext;

        public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
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
                await LogErrorInDatabase(context, ex);
                await HandleExceptionAsync(context, ex);
            }
        }


        private async Task LogErrorInDatabase(HttpContext context, Exception exception)
        {
            var errorLog = new ErrorLog
            {
                Id = Guid.NewGuid().ToString(),
                Message = exception.Message,
                StackTrace = exception.StackTrace,
                Source = exception.Source,
                LogDate = DateTime.UtcNow,
                Url = context.Request.Path,
                ExceptionType = exception.GetType().Name,
                InnerException = exception.InnerException?.ToString(),
                HttpMethod = context.Request.Method,
                RequestHeaders = string.Join(", ", context.Request.Headers.Select(h => $"{h.Key}: {h.Value}")),
                UserIpAddress = context.Connection.RemoteIpAddress?.ToString(),
                UserId = context.User?.FindFirst("UserId")?.Value,
                FormData = " "
            };

            try
            {
                await _dbContext.ErrorLogs.AddAsync(errorLog);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Failed to log error in database: {Message}. Inner Exception: {InnerException}", dbEx.Message, dbEx.InnerException?.Message);

            }

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
                // SQL-related Exception Cases
                case DbUpdateException dbUpdateException:
                    if (dbUpdateException.InnerException is NpgsqlException npgsqlEx)
                    {
                        switch (npgsqlEx.SqlState)
                        {
                            case "23503": // Foreign key violation
                                message = "Operation Failed: This record cannot be deleted or updated because it is referenced by another record in the database.";
                                title = "Foreign Key Violation";
                                status = HttpStatusCode.Conflict;
                                break;

                            case "23505": // Unique constraint violation
                                message = "Operation Failed: Duplicate entry detected. The data you are trying to save already exists.";
                                title = "Unique Constraint Violation";
                                status = HttpStatusCode.Conflict;
                                break;

                            case "23502": // Not-null violation
                                message = "Operation Failed: A required field is missing. Please provide all necessary values.";
                                title = "Not-Null Constraint Violation";
                                status = HttpStatusCode.BadRequest;
                                break;

                            case "42P01": // Undefined table
                                message = "Operation Failed: The specified table does not exist in the database.";
                                title = "Invalid Table Name";
                                status = HttpStatusCode.InternalServerError;
                                break;

                            case "40001": // Serialization failure / deadlock
                                message = "Operation Failed: The database is busy or a deadlock occurred. Please try the operation again.";
                                title = "Database Deadlock / Concurrency Error";
                                status = HttpStatusCode.InternalServerError;
                                break;

                            default:
                                message = $"A PostgreSQL database error occurred. Code: {npgsqlEx.SqlState}, Message: {npgsqlEx.Message}";
                                title = "Database Error";
                                status = HttpStatusCode.InternalServerError;
                                break;
                        }
                    }
                    else
                    {
                        // General database update error
                        message = "An unexpected database error occurred. Please contact support.";
                        title = "Database Update Error";
                        status = HttpStatusCode.InternalServerError;
                    }
                    break;





                case NpgsqlException sqlExceptio:
                    message = $"A SQL database error occurred: {sqlExceptio.Message}";
                    status = HttpStatusCode.InternalServerError;
                    title = "SQL Database Error Occurred";
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