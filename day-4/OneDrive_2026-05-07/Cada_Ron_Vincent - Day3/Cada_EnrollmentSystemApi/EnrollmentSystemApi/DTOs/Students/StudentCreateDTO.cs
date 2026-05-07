namespace EnrollmentSystemApi.DTOs.Students;

public class StudentCreateDTO
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Gender { get; set; } = string.Empty;
    public int SectionId { get; set; }
    public int GeneratedGrade { get; set;}
}