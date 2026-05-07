using Day3WebApi;
using Day3WebApi.MiddlewareClasses;
using Day3WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IBooksService, BooksService>();
builder.Services.AddSingleton<RequestCounterService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

//app.Use(async (context, next) =>
//{
//    Console.WriteLine("Middleware 1 Req");
//    await next();

//    Console.WriteLine("Middleware 1 Res");
//});

//app.UseMiddleware2();
app.UseMiddleware<Middleware3>();
app.UseMiddleware<RequestModifier>();

app.UseExceptionHandler(err =>
{
    err.Run(async (context) =>
    {
        context.Response.StatusCode = 200;
        await context.Response.WriteAsync("Hala nasira!");
    });
});
app.MapControllers();

app.MapGet("/api/counter", (RequestCounterService counter) =>
{
    return Results.Ok(counter.Counter);
});

app.Run();
