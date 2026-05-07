using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class RandomGradeGeneratorMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly Random _random = new Random();

    public RandomGradeGeneratorMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var grade = _random.Next(75, 101);
        context.Response.Headers["GeneratedGrade"] = grade.ToString();
        await _next(context);
    }
}