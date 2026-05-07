using EnrollmentSystemApi.Data;
using System.Net.Cache;
using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddSingleton<InMemoryEnrollmentStore>();
builder.Services.AddSingleton<EnrollmentSystemApi.Services.Students.IStudentService, EnrollmentSystemApi.Services.Students.StudentService>();
builder.Services.AddSingleton<EnrollmentSystemApi.Services.Sections.ISectionService, EnrollmentSystemApi.Services.Sections.SectionService>();

var app = builder.Build();

// app.Use(async (context, next) =>
// {
//     context.Request.EnableBuffering();

//     if (context.Request.ContentLength > 0 && context.Request.ContentType?.Contains("application/json") == true)
//     {
//         using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
//         var body = await reader.ReadToEndAsync();

//         var jsonObject = JsonSerializer.Deserialize<Dictionary<string, object>>(body);
//         if (jsonObject != null)
//         {
//             jsonObject["GeneratedGrade"] = new Random().Next(75, 101);

//             var modifiedBody = JsonSerializer.Serialize(jsonObject);
//             var byteArray = Encoding.UTF8.GetBytes(modifiedBody);
//             context.Request.Body = new MemoryStream(byteArray);
//             context.Request.ContentLength = byteArray.Length;
//         }

//         context.Request.Body.Position = 0;
//         await next(context);
//     }
// });

// app.UseMiddleware<RandomGradeGeneratorMiddleware>();
// app.UseRandomGradeGenerator();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();