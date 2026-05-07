namespace EnrollmentSystemApi.DTOs.Students;

public class StudentResponseDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string SectionCode { get; set; } = string.Empty;
    public int GeneratedGrade { get; set; }
}
