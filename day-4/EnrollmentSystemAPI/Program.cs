using EnrollmentSystem.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddSingleton<EnrollmentSystem.Services.Students.IStudentServices, EnrollmentSystem.Services.Students.StudentServices>();
builder.Services.AddSingleton<EnrollmentSystem.Services.Sections.ISectionsService, EnrollmentSystem.Services.Sections.SectionServices>();
builder.Services.AddTransient<FactoryMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}



// Inline middleware
app.Use(async (context, next) =>
{
    if (HttpMethods.IsPost(context.Request.Method))
    {
        int grade = Random.Shared.Next(75, 101);
        context.Items["GeneratedGrade"] = grade;
    }
    await next();
});

// Class-based middleware
app.UseMiddleware<ClassMiddleware>();

// Factory-based middleware
app.UseMiddleware<FactoryMiddleware>();

app.MapControllers();
app.UseHttpsRedirection();

app.Run();