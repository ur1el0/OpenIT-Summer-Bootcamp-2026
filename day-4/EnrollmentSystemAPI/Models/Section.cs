using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace EnrollmentSystemApi.Models
{
    public class Section
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public List<Student> Students { get; set; } = new List<Student>();
    }
}