using Day3WebApi.Services;

namespace Day3WebApi.MiddlewareClasses
{
    public class Middleware3
    {

        private readonly RequestDelegate next;
        private readonly RequestCounterService counterService;

        public Middleware3(RequestDelegate next, RequestCounterService counterService)
        {
            this.next = next;
            this.counterService = counterService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            this.counterService.Increment();
            Console.WriteLine("Middleware 3 Req");
            await next(context);
            Console.WriteLine("Middleware 3 Res");
        }
    }
}
