using System.Net.Cache;
using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

public class RandomGradeGeneratorMiddleware
{
    private readonly RequestDelegate _next;

    public RandomGradeGeneratorMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public int GenerateRandomGrade()
    {
        var random = new Random();
        return random.Next(75, 101);
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.EnableBuffering();

        if (context.Request.ContentLength > 0 && context.Request.ContentType?.Contains("application/json") == true)
        {
            using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
            var body = await reader.ReadToEndAsync();

            var jsonObject = JsonSerializer.Deserialize<Dictionary<string, object>>(body);
            if (jsonObject != null)
            {
                jsonObject["GeneratedGrade"] = GenerateRandomGrade();

                var modifiedBody = JsonSerializer.Serialize(jsonObject);
                var byteArray = Encoding.UTF8.GetBytes(modifiedBody);
                context.Request.Body = new MemoryStream(byteArray);
                context.Request.ContentLength = byteArray.Length;
            }
        }

        context.Request.Body.Position = 0;
        await _next(context);
    }
}