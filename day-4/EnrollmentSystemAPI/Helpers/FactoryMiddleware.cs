using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace EnrollmentSystem.Helpers
{
    public class FactoryMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Response.Headers.Append("X-Factory-Middleware", "Active");
            await next(context);
        }
    }
}
