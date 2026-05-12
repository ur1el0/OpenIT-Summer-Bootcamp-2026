using System.ComponentModel.DataAnnotations;

namespace BootcampEF;

public class Student
{
    public int Id {get;set;}
    [Required]
    [MaxLength(50)]
    public string Name {get;set;} = String.Empty;
    public int CourseId { get; set; } // Foreign Key
    public Course? Course { get; set; }  // Navigation Property - lets us access the course data
}