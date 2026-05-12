using System;
using System.Collections.Generic;
namespace EnrollmentSystem.Data
{
    using EnrollmentSystem;

    public class SeedData
    {
        public readonly List<Students> students = new()
        {
            new Students { FirstName = "Roosc", LastName = "Zaño", Year = 3, Gender = "Male", Enrolled = true },
            new Students { FirstName = "Ron Vincent", LastName = "Cada", Year = 3, Gender = "Female", Enrolled = true },
            new Students { FirstName = "Mike Andrei", LastName = "Gomez", Year = 4, Gender = "Female", Enrolled = false },
            new Students { FirstName = "Kurt Patrick", LastName = "Laja", Year = 2, Gender = "Male", Enrolled = true }
        };

        public readonly List<Programs> programs = new()
        {
            new Programs { ProgramName = "BS in Computer Science" },
            new Programs { ProgramName = "BS in Information Technology" },
            new Programs { ProgramName = "BS in Entertainment and Multimedia Computing" }
        };

        public readonly List<Sections> sections = new()
        {
            new Sections { Code = "M001", Year = 2020, ProgramId = 1 },
            new Sections { Code = "M002", Year = 2022, ProgramId = 2 },
            new Sections { Code = "M003", Year = 2022, ProgramId = 3 },
            new Sections { Code = "M004", Year = 2021, ProgramId = 1 }
        };

        public readonly List<Student_Sections> student_Sections = new()
        {
            new Student_Sections { StudentId = 1, SectionId = 1 },
            new Student_Sections { StudentId = 2, SectionId = 2 },
            new Student_Sections { StudentId = 3, SectionId = 3 },
            new Student_Sections { StudentId = 4, SectionId = 4 }
        };

        public readonly List<StudentGrades> studentGrades = new()
        {
            new StudentGrades { StudentId = 1, grade = 100 },
            new StudentGrades { StudentId = 1, grade = 100 },
            new StudentGrades { StudentId = 1, grade = 100 },
            new StudentGrades { StudentId = 2, grade = 75 },
            new StudentGrades { StudentId = 2, grade = 75 },
            new StudentGrades { StudentId = 2, grade = 75 },
            new StudentGrades { StudentId = 3, grade = 75 },
            new StudentGrades { StudentId = 3, grade = 75 },
            new StudentGrades { StudentId = 3, grade = 75 },
            new StudentGrades { StudentId = 4, grade = 100 },
            new StudentGrades { StudentId = 4, grade = 100 },
            new StudentGrades { StudentId = 4, grade = 100 }
        };
    }
}
