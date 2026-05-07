using EnrollmentSystemApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddSingleton<InMemoryEnrollmentStore>();
builder.Services.AddSingleton<EnrollmentSystemApi.Services.Students.IStudentService, EnrollmentSystemApi.Services.Students.StudentService>();
builder.Services.AddSingleton<EnrollmentSystemApi.Services.Sections.ISectionService, EnrollmentSystemApi.Services.Sections.SectionService>();

var app = builder.Build();

app.Use(async (context, next ) =>
{
    if (context.Request.Method == "POST" &&
        context.Request.Path.Value!.Contains("/api/section"))
    {
        var random = new Random();
        int grade = random.Next(75, 101);
        context.Items["GeneratedGrade"] = grade;
    }
    await next(context);

});


app.UseMiddleware<RandomGradeGeneratorMiddleware>();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();