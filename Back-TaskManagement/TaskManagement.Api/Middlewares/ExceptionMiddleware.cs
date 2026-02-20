using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace TaskManagement.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (OperationCanceledException oce)
            {
                _logger.LogWarning(oce, "Request was canceled by the client.");

                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = 499;
                }

                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            if (context.Response.HasStarted)
            {
                _logger.LogWarning(ex, "The response has already started, the exception middleware will not write the response. Path: {Path}", context.Request.Path);
                return;
            }

            int statusCode = ex switch
            {
                KeyNotFoundException => StatusCodes.Status404NotFound,
                InvalidOperationException => StatusCodes.Status400BadRequest,
                ArgumentException => StatusCodes.Status400BadRequest,
                JsonException => StatusCodes.Status400BadRequest,
                UnauthorizedAccessException => StatusCodes.Status403Forbidden,
                DbUpdateConcurrencyException => StatusCodes.Status409Conflict,
                DbUpdateException => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

            var problem = new ProblemDetails
            {
                Status = statusCode,
                Title = GetTitle(statusCode),
                Detail = statusCode == StatusCodes.Status500InternalServerError && !_env.IsDevelopment()
                    ? "An unexpected error occurred."
                    : ex.Message,
                Instance = context.Request.Path
            };

            problem.Extensions["requestId"] = context.TraceIdentifier;
            problem.Extensions["timestampUtc"] = DateTime.UtcNow;

            if (_env.IsDevelopment())
            {
                problem.Extensions["stackTrace"] = ex.StackTrace;
                problem.Extensions["exceptionType"] = ex.GetType().Name;
                if (ex.InnerException != null)
                    problem.Extensions["innerException"] = ex.InnerException.Message;
            }

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsJsonAsync(problem);
        }

        private static string GetTitle(int statusCode) =>
            statusCode switch
            {
                400 => "Bad Request",
                401 => "Unauthorized",
                403 => "Forbidden",
                404 => "Not Found",
                409 => "Conflict",
                500 => "Internal Server Error",
                _ => "Error"
            };
    }
}
