using System.Net.Cache;
using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;


public static class RandomGradeGeneratorMiddlewareExtension
{
    public static int GenerateRandomGrade()
    {
        var random = new Random();
        return random.Next(75, 101);
    }
    public static IApplicationBuilder UseRandomGradeGenerator(this WebApplication app)
    {
        return app.Use(async (context, next) =>
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

                context.Request.Body.Position = 0;
                await next(context);
            }
        });
    }
}