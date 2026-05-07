using System.ComponentModel.DataAnnotations;

namespace EnrollmentSystemApi.models;
public class Section
{
    public int Id { get; set; }
    [Required]
    public string Code { get; set; } = string.Empty;
    public List<Student> Students { get; set; } = [];

}