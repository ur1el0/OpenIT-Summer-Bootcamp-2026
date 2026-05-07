using System.ComponentModel.DataAnnotations;

namespace EnrollmentSystemApi.models;
public class Student
{
    public int Id { get; set; }
    [Required]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    public string LastName { get; set; } = string.Empty;
    [Required]
    public int Age { get; set; }
    [Required]
    public string Gender { get; set; } = string.Empty;
    [Required]
    public int SectionId { get; set; }
}