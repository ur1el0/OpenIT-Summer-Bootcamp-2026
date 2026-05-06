using System.ComponentModel.DataAnnotations;

namespace EnrollmentSystem.Models
{
    public class Section
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Student> Students { get; set; } = [];
    }
}