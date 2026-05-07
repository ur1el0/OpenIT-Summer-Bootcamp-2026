namespace EnrollmentSystemApi.DTOs.Students;

public class StudentPatchDTO
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? Age { get; set; }
    public string? Gender { get; set; }
    public int? SectionId { get; set; }
    public int? GeneratedGrade { get; set; }
}
