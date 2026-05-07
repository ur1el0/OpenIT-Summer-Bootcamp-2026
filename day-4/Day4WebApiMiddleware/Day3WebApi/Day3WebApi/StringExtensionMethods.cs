namespace Day3WebApi
{
    public static class StringExtensionMethods
    {
        public static string ToCamelCase(string value, bool sureKanaba)
        {
            if (sureKanaba)
            {
                return value + " alala camel case kana";
            }
            return value;
        }

        public static IApplicationBuilder UseMiddleware2(this WebApplication app)
        {
            return app.Use(async (context, next) =>
            {
                Console.WriteLine("Middleware 2 Req");

                if (context.Request.Path.ToString() == "/api/blocked")
                {
                    context.Response.StatusCode = 403;
                    await context.Response.WriteAsync("Di ka pwede");
                    return;
                }
                else
                {
                    await next();
                }

                Console.WriteLine("Middleware 2 Res");
            });
        }
    }
}
