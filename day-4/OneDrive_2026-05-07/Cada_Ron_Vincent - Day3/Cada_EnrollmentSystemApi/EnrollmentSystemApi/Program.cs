var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSingleton<EnrollSystem.Data.InMemoryEnrollmentStore>();
builder.Services.AddSingleton<EnrollmentSystem.Services.Students.IStudentServices, EnrollmentSystem.Services.Students.StudentServices>();
builder.Services.AddSingleton<EnrollmentSystem.Services.Sections.ISectionsService, EnrollmentSystem.Services.Sections.SectionServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}



app.UseHttpsRedirection();
app.MapControllers();

app.Run();
