namespace StudentEnrollmentAPI.Models;

public class AuthUser
{
	public string UserName { get; set; } = string.Empty;
	public string PasswordHash { get; set; } = string.Empty;
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}