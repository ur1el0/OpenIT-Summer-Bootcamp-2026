using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentEnrollmentAPI.Services;

namespace StudentEnrollmentAPI.Controllers;

[ApiController]
[Route("")]
public class AuthController : ControllerBase
{
	private readonly AuthStore _authStore;

	public AuthController(AuthStore authStore)
	{
		_authStore = authStore;
	}

	public sealed record AuthRequest(string Username, string Password);

	[HttpPost("register")]
	[AllowAnonymous]
	public async Task<IActionResult> Register([FromBody] AuthRequest request)
	{
		try
		{
			var user = await _authStore.RegisterAsync(request.Username, request.Password);
			return Created("/auth/me", new { userName = user.UserName });
		}
		catch (ArgumentException exception)
		{
			return BadRequest(new { message = exception.Message });
		}
		catch (InvalidOperationException exception)
		{
			return Conflict(new { message = exception.Message });
		}
	}

	[HttpPost("login")]
	[AllowAnonymous]
	public async Task<IActionResult> Login([FromQuery(Name = "useCookies")] bool useCookies = true, [FromBody] AuthRequest request = default!)
	{
		if (!useCookies)
		{
			return BadRequest(new { message = "Cookie-based login is required." });
		}

		var user = await _authStore.ValidateAsync(request.Username, request.Password);
		if (user is null)
		{
			return Unauthorized(new { message = "Invalid username or password." });
		}

		var claims = new List<Claim>
		{
			new(ClaimTypes.Name, user.UserName),
		};
		var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
		var principal = new ClaimsPrincipal(identity);

		await HttpContext.SignInAsync(
			CookieAuthenticationDefaults.AuthenticationScheme,
			principal,
			new AuthenticationProperties
			{
				IsPersistent = true,
				AllowRefresh = true,
			});

		return Ok(new { userName = user.UserName });
	}

	[HttpPost("logout")]
	[Authorize]
	public async Task<IActionResult> Logout()
	{
		await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
		return NoContent();
	}

	[HttpGet("auth/me")]
	[Authorize]
	public IActionResult Me()
	{
		return Ok(new { userName = User.Identity?.Name ?? string.Empty });
	}
}
