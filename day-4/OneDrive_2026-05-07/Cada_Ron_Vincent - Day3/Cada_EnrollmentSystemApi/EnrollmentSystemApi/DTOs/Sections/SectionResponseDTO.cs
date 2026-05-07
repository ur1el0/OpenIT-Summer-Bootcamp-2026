namespace EnrollmentSystemApi.DTOs.Sections;

public class SectionResponseDTO
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public int StudentCount { get; set; }
}