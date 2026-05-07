using System.Collections.Generic;
using EnrollmentSystemApi.Models;

namespace EnrollmentSystemApi.Data
{
    public class InMemoryEnrollmentStore
    {
        public List<Section> Sections { get; } = new List<Section>
        {
            new Section { Id = 1, Code = "MB02", Students = new List<Student>() }
        };

        public List<Student> Students { get; } = new List<Student>
        {
            new Student { Id = 1, FirstName = "Roosc", LastName = "Zaño", Age = 21, SectionId = 1, Gender = "M", Grades = 90 },
            new Student { Id = 2, FirstName = "Mike", LastName = "Zaño", Age = 31, SectionId = 1, Gender = "M", Grades = 85 }
        };
    }
}