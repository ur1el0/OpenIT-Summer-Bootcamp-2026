using System.ComponentModel.DataAnnotations;
<<<<<<< HEAD
using System.Collections.Generic;

namespace EnrollmentSystemApi.Models
=======

namespace EnrollmentSystem.Models
>>>>>>> fbffcb53571c48c8df295b2262b2029a1ff37dba
{
    public class Section
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
<<<<<<< HEAD
        public List<Student> Students { get; set; } = new List<Student>();
=======
        public List<Student> Students { get; set; } = [];
>>>>>>> fbffcb53571c48c8df295b2262b2029a1ff37dba
    }
}