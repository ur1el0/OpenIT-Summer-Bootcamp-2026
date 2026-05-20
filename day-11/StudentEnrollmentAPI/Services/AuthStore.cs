using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using StudentEnrollmentAPI.Models;

namespace StudentEnrollmentAPI.Services;

public class AuthStore
{
	private static readonly JsonSerializerOptions JsonOptions = new()
	{
		WriteIndented = true,
		PropertyNameCaseInsensitive = true,
	};

	private readonly SemaphoreSlim _gate = new(1, 1);
	private readonly PasswordHasher<AuthUser> _passwordHasher = new();
	private readonly string _storePath;

	public AuthStore(IWebHostEnvironment environment)
	{
		_storePath = Path.Combine(environment.ContentRootPath, "App_Data", "users.json");
	}

	public async Task<AuthUser> RegisterAsync(string username, string password)
	{
		var normalizedUserName = NormalizeUserName(username);
		if (string.IsNullOrWhiteSpace(password))
		{
			throw new ArgumentException("Password is required.", nameof(password));
		}

		await _gate.WaitAsync();
		try
		{
			var users = await LoadUsersAsync();
			if (users.Any(user => string.Equals(user.UserName, normalizedUserName, StringComparison.OrdinalIgnoreCase)))
			{
				throw new InvalidOperationException("A user with that username already exists.");
			}

			var user = new AuthUser
			{
				UserName = normalizedUserName,
				CreatedAt = DateTime.UtcNow,
			};

			user.PasswordHash = _passwordHasher.HashPassword(user, password);
			users.Add(user);
			await SaveUsersAsync(users);
			return user;
		}
		finally
		{
			_gate.Release();
		}
	}

	public async Task<AuthUser?> ValidateAsync(string username, string password)
	{
		var normalizedUserName = NormalizeUserName(username);
		if (string.IsNullOrWhiteSpace(password))
		{
			return null;
		}

		await _gate.WaitAsync();
		try
		{
			var users = await LoadUsersAsync();
			var user = users.FirstOrDefault(item => string.Equals(item.UserName, normalizedUserName, StringComparison.OrdinalIgnoreCase));
			if (user is null)
			{
				return null;
			}

			var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
			return result == PasswordVerificationResult.Success ? user : null;
		}
		finally
		{
			_gate.Release();
		}
	}

	public async Task<AuthUser?> FindByUserNameAsync(string username)
	{
		var normalizedUserName = NormalizeUserName(username);

		await _gate.WaitAsync();
		try
		{
			var users = await LoadUsersAsync();
			return users.FirstOrDefault(item => string.Equals(item.UserName, normalizedUserName, StringComparison.OrdinalIgnoreCase));
		}
		finally
		{
			_gate.Release();
		}
	}

	private static string NormalizeUserName(string username)
	{
		if (string.IsNullOrWhiteSpace(username))
		{
			throw new ArgumentException("Username is required.", nameof(username));
		}

		return username.Trim();
	}

	private async Task<List<AuthUser>> LoadUsersAsync()
	{
		if (!File.Exists(_storePath))
		{
			return [];
		}

		var json = await File.ReadAllTextAsync(_storePath);
		if (string.IsNullOrWhiteSpace(json))
		{
			return [];
		}

		return JsonSerializer.Deserialize<List<AuthUser>>(json, JsonOptions) ?? [];
	}

	private async Task SaveUsersAsync(List<AuthUser> users)
	{
		var directory = Path.GetDirectoryName(_storePath);
		if (!string.IsNullOrWhiteSpace(directory))
		{
			Directory.CreateDirectory(directory);
		}

		var json = JsonSerializer.Serialize(users, JsonOptions);
		await File.WriteAllTextAsync(_storePath, json);
	}
}