using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EnrollmentSystem.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(255, MinimumLength = 1, ErrorMessage ="Name must be 1-255 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required")]
        [StringLength(255, MinimumLength = 1, ErrorMessage ="Name must be 1-255 characters")]
        public string LastName { get; set; } = string.Empty;

        public int SectionId { get; set; }

        public int Age { get; set; }

        public char Gender { get; set; }

        public int Grade { get; set; }
    }
}