using Microsoft.AspNetCore.Builder;

namespace WebParser.Api.Common
{
    public static class ExceptionMiddlewareExtensions
    {
        /// <summary>
        /// Extend IApplicationBuilder to add custome middleware
        /// </summary>
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
