using StudentEnrollmentAPI;
using StudentEnrollmentAPI.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
	options.AddPolicy("Frontend", policy =>
	{
		policy.WithOrigins("http://localhost:5173")
			.AllowAnyMethod()
			.AllowAnyHeader()
			.AllowCredentials();
	});
});

builder.Services.AddSingleton<AuthStore>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>
	{
		options.Cookie.Name = "studentenrollment.auth";
		options.Cookie.HttpOnly = true;
		options.Cookie.SameSite = SameSiteMode.Lax;
		options.Cookie.SecurePolicy = CookieSecurePolicy.None;
		options.Events.OnRedirectToLogin = context =>
		{
			context.Response.StatusCode = StatusCodes.Status401Unauthorized;
			return Task.CompletedTask;
		};
		options.Events.OnRedirectToAccessDenied = context =>
		{
			context.Response.StatusCode = StatusCodes.Status403Forbidden;
			return Task.CompletedTask;
		};
	});
builder.Services.AddAuthorization();
builder.Services.AddDbContext<EnrollmentContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.Run();

