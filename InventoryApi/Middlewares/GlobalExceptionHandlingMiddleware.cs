using InventoryApi.DataContext;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.RegularExpressions;

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
                    if (dbUpdateException.InnerException is SqlException sqlException)
                    {
                        switch (sqlException.Number)
                        {
                            case 547: // Foreign key violation
                                if (sqlException.Message.Contains("DELETE"))
                                {
                                    message = "Delete Failed: Unable to delete the item because it is referenced by another record. Please remove any related data before attempting to delete.";
                                    title = "Foreign Key Violation - Delete Conflict";
                                }
                                else if (sqlException.Message.Contains("INSERT"))
                                {
                                    string sourceTableName = "the source table";
                                    string sourceColumnName = "the source column";
                                    string targetTableName = "the target table";
                                    string targetColumnName = "the target column";

                                    // Pattern to match the foreign key constraint and extract the source table, source column, and target table
                                    var match = Regex.Match(sqlException.Message, @"constraint \""FK_(\w+)_(\w+)_(\w+)\""");

                                    if (match.Success)
                                    {
                                        sourceTableName = match.Groups[1].Value;   // "CartItems"
                                        sourceColumnName = match.Groups[3].Value;  // "ProductID"
                                        targetTableName = match.Groups[2].Value;   // "Products"
                                    }

                                    // Extract target column from the error message
                                    var matchTargetColumn = Regex.Match(sqlException.Message, @"column '(\w+)'");

                                    if (matchTargetColumn.Success)
                                    {
                                        targetColumnName = matchTargetColumn.Groups[1].Value;
                                    }

                                    // Custom error message
                                    message = $"Creation Failed: The operation could not be completed because the record you are attempting to add to the '{sourceTableName}' table (column: '{sourceColumnName}') references a non-existing item in the '{targetTableName}' table (column: '{targetColumnName}'). Please ensure that the referenced item exists in the related table before retrying.";
                                    title = "Foreign Key Violation - Insert Conflict";
                                }



                                else if (sqlException.Message.Contains("UPDATE"))
                                {
                                    message = "Update Failed: The record you are trying to update references a non-existing item. Ensure all related data exists before proceeding.";
                                    title = "Foreign Key Violation - Update Conflict";
                                }
                                else
                                {
                                    message = "Operation Failed: A foreign key constraint violation occurred. Please check your data and try again.";
                                    title = "Foreign Key Violation";
                                }
                                status = HttpStatusCode.Conflict;
                                break;

                            case 2601: // Unique index violation
                            case 2627: // Violation of primary key constraint
                                message = "Save Failed: The data you are trying to save already exists. Duplicate entries are not allowed. Please ensure the data is unique and try again.";
                                title = "Duplicate Key Violation";
                                status = HttpStatusCode.Conflict;
                                break;

                            case 515: // Cannot insert null into a non-nullable column
                                message = "Save Failed: A required field is missing. Please ensure all mandatory fields are filled out and try again.";
                                title = "Null Value Violation";
                                status = HttpStatusCode.BadRequest;
                                break;

                            case 208: // Invalid object name
                                message = "Operation Failed: The specified table or object does not exist in the database. Please verify the database schema and try again.";
                                title = "Invalid Object Name";
                                status = HttpStatusCode.InternalServerError;
                                break;

                            case 1205: // Deadlock victim
                                message = "Operation Failed: The database is experiencing high contention, and your request was selected as a deadlock victim. Please try the operation again.";
                                title = "Deadlock Occurred";
                                status = HttpStatusCode.InternalServerError;
                                break;

                            default:
                                // General SQL exception handling
                                message = $"Operation Failed: A database error occurred. Error Code: {sqlException.Number}. Please contact support if the issue persists.";
                                title = "Database Error";
                                status = HttpStatusCode.InternalServerError;
                                break;
                        }
                    }
                    else
                    {
                        // General database update error
                        message = "Save Failed: An unexpected error occurred while processing your request. Please try again or contact support.";
                        title = "Database Update Error";
                        status = HttpStatusCode.Conflict;
                    }
                    break;




                case SqlException sqlExceptio:
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