namespace StudentEnrollmentAPI.Model.Dto;

public class CreateStudentDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Course { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string OldSchool { get; set; } = string.Empty;
    public string Enrolled { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}

public class UpdateStudentDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? Year { get; set; }
    public string? Course { get; set; }
    public string? Gender { get; set; }
    public string? OldSchool { get; set; }
    public string? Enrolled { get; set; }
    public string? Department { get; set; }
    public string? Address { get; set; }
}
