using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace WebParser.Api.Common
{
    /// <summary>
    /// Exception middleware for handling error and provide a friendly error response
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ScanException ex)
            {
                _logger.LogError($"There is some unhandled exception: {ex}");
                await HandleExceptionAsync(context: httpContext, message: ex.Message, statusCode: ex.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError($"There is some unhandled exception: {ex}");
                await HandleExceptionAsync(httpContext);
            }

        }
        private Task HandleExceptionAsync(HttpContext context, string message = "", int statusCode = StatusCodes.Status500InternalServerError)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsync(new ErrorDetail()
            {
                StatusCode = context.Response.StatusCode,
                Message = string.IsNullOrEmpty(message) ? "Internal Server Error. Contact support" : message
            }.ToString());
        }
    }
}
