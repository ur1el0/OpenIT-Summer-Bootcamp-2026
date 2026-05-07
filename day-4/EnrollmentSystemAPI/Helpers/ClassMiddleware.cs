using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace EnrollmentSystem.Helpers
{
    public class ClassMiddleware
    {
        private readonly RequestDelegate _next;
        public ClassMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            // Example: Add a custom header
            context.Response.Headers.Add("X-Class-Middleware", "Active");
            await _next(context);
        }
    }
}
