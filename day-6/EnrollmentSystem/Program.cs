using System;
using EnrollmentSystem;
using EnrollmentSystem.Data;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

class Program
{
    static void Main()
    {
        using var context = new EnrollmentContext();
        context.Database.Migrate();
        if (!context.Students.Any()) SeedFromSeedData(context);

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("Select an option:");
            Console.WriteLine("1. Add students");
            Console.WriteLine("2. Get all students");
            Console.WriteLine("3. Add programs");
            Console.WriteLine("4. Get all programs");
            Console.WriteLine("5. Add sections");
            Console.WriteLine("6. Get all sections");
            Console.WriteLine("7. Add student-section relationships");
            Console.WriteLine("8. Get all student-section relationships");
            Console.WriteLine("9. Add student grades");
            Console.WriteLine("10. Get all student grades");
            Console.WriteLine("11. Exit");
            var enrollmentCRUD = new EnrollmentCRUD();

            do
            {
                Console.WriteLine("\nSelect an option:");
                Console.WriteLine("1. Add all students");
                Console.WriteLine("2. Get all students");
                Console.WriteLine("3. Add all programs");
                Console.WriteLine("4. Get all programs");
                Console.WriteLine("5. Add all sections");
                Console.WriteLine("6. Get all sections");
                Console.WriteLine("7. Add student-section relationships");
                Console.WriteLine("8. Get all student-section relationships");
                Console.WriteLine("9. Add all student grades");
                Console.WriteLine("10. Get all student grades");
                Console.WriteLine("11. Exit");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        enrollmentCRUD.AddAllStudents();
                        break;
                    case "2":
                        enrollmentCRUD.GetAllStudents();
                        break;
                    case "3":
                        enrollmentCRUD.AddAllPrograms();
                        break;
                    case "4":
                        enrollmentCRUD.GetAllPrograms();
                        break;
                    case "5":
                        enrollmentCRUD.AddAllSections();
                        break;
                    case "6":
                        enrollmentCRUD.GetAllSections();
                        break;
                    case "7":
                        enrollmentCRUD.AddStudent_Section();
                        break;
                    case "8":
                        enrollmentCRUD.GetAllStudent_Sections();
                        break;
                    case "9":
                        enrollmentCRUD.AddAllStudentGrades();
                        break;
                    case "10":
                        enrollmentCRUD.GetAllStudentGrades();
                        break;
                    case "11":
                        return;
                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }
            } while (true);
        }
    }

    private static void SeedFromSeedData(EnrollmentContext context)
    {
        throw new NotImplementedException();
    }

    public class EnrollmentCRUD
    {
        private readonly EnrollmentContext context;
        private readonly SeedData seedData;

        public EnrollmentCRUD()
        {
            context = new EnrollmentContext();
            seedData = new SeedData();
        }

        public void AddAllStudents()
        {
            var students = seedData.students
                .Where(s =>
                    !string.IsNullOrWhiteSpace(s.FirstName) &&
                    !string.IsNullOrWhiteSpace(s.LastName) &&
                    !string.IsNullOrWhiteSpace(s.Gender))
                .Where(s => !context.Students.Any(existing =>
                    existing.FirstName == s.FirstName &&
                    existing.LastName == s.LastName &&
                    existing.Year == s.Year &&
                    existing.Gender == s.Gender &&
                    existing.Enrolled == s.Enrolled))
                .ToList();

            foreach (var s in students)
            {
                context.Students.Add(s);
            }

            if (!students.Any())
            {
                Console.WriteLine("No new students added.");
                return;
            }

            context.SaveChanges();
            Console.WriteLine($"{students.Count} student(s) added.");
        }

        public void GetAllStudents()
        {
            var students = context.Students.ToList();
            foreach (var student in students)
            {
                Console.WriteLine($"ID: {student.Id}, Name: {student.FirstName} {student.LastName}, Year: {student.Year}, Gender: {student.Gender}, Enrolled: {student.Enrolled}");
            }
        }

        public void AddAllPrograms()
        {
            var programs = seedData.programs
                .Where(p => !string.IsNullOrWhiteSpace(p.ProgramName))
                .Where(p => !context.Programs.Any(existing => existing.ProgramName == p.ProgramName))
                .ToList();

            foreach (var p in programs)
            {
                context.Programs.Add(p);
            }

            if (!programs.Any())
            {
                Console.WriteLine("No new programs added.");
                return;
            }

            context.SaveChanges();
            Console.WriteLine($"{programs.Count} program(s) added.");
        }

        public void GetAllPrograms()
        {
            var programs = context.Programs.ToList();
            foreach (var program in programs)
            {
                Console.WriteLine($"ID: {program.Id}, Name: {program.ProgramName}");
            }
        }

        public void AddAllSections()
        {
            var sections = seedData.sections;
            foreach (var s in sections)
            {
                context.Sections.Add(s);
            }
            context.SaveChanges();
            Console.WriteLine("All sections added.");
        }

        public void GetAllSections()
        {
            var sections = context.Sections.ToList();
            foreach (var section in sections)
            {
                Console.WriteLine($"ID: {section.Id}, Name: {section.Code}");
            }
        }

        public void AddStudent_Section()
        {
            var studentSections = seedData.student_Sections;
            foreach (var ss in studentSections)
            {
                context.Student_Sections.Add(ss);
            }
            context.SaveChanges();
            Console.WriteLine("All student-section relationships added.");
        }

        public void GetAllStudent_Sections()
        {
            var studentSections = context.Student_Sections.ToList();
            foreach (var ss in studentSections)
            {
                Console.WriteLine($"ID: {ss.Id}, Student ID: {ss.StudentId}, Section ID: {ss.SectionId}, Enrolled At: {ss.EnrolledAt}");
            }
        }

        public void AddAllStudentGrades()
        {
            var studentGrades = seedData.studentGrades;
            foreach (var sg in studentGrades)
            {
                context.StudentGrades.Add(sg);
            }
            context.SaveChanges();
            Console.WriteLine("All student grades added.");
        }

        public void GetAllStudentGrades()
        {
            var studentGrades = context.StudentGrades
                .Include(g => g.Student)
                .ToList();
            foreach (var sg in studentGrades)
            {
                Console.WriteLine($"Full Name: {sg.Student?.FirstName} {sg.Student?.LastName}, Grade: {sg.grade}");
            }
        }

        public Students? GetStudent(int id)
        {
            return context.Students.Find(id);
        }

        public void UpdateStudent(Students student)
        {
            context.Students.Update(student);
            context.SaveChanges();
        }

        public void DeleteStudent(int id)
        {
            var student = context.Students.Find(id);
            if (student != null)
            {
                context.Students.Remove(student);
                context.SaveChanges();
            }
        }
    }
}