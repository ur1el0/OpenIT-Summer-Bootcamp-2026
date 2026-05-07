using Day3WebApi.Services;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Day3WebApi.MiddlewareClasses
{
    public class RequestModifier
    {
        private readonly RequestDelegate next;

        public RequestModifier(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            using StreamReader reader = new(context.Request.Body, leaveOpen: false);
            var oldRequest = await reader.ReadToEndAsync();

            var requestData = Encoding.UTF8.GetBytes(oldRequest.Replace("Bootcamp 1", "Bootcamp 30"));
            var stream = new MemoryStream(requestData);

             context.Request.Body = stream;


            await next(context);
        }
    }
}
