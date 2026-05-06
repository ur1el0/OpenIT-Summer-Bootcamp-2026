var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddScoped<EnrollmentSystem.Services.Student.IStudentServices, EnrollmentSystem.Services.Student.StudentServices>();
builder.Services.AddScoped<EnrollmentSystem.Services.Sections.ISectionsService, EnrollmentSystem.Services.Sections.SectionServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();
app.UseHttpsRedirection();

app.Run();