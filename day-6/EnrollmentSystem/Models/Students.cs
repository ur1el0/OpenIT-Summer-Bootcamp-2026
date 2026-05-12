using System.ComponentModel.DataAnnotations;

namespace EnrollmentSystem;

public class Students
{
    public int Id { get;set; }

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = "";

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = "";

    public int Year { get; set; }

    [MaxLength(50)]
    public string Gender { get; set; } = "";
    public bool Enrolled { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}