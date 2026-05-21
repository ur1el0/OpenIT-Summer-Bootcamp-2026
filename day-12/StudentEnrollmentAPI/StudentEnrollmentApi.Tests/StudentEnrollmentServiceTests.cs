using System;
using System.Linq;
using EnrollmentSystem;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentAPI;
using StudentEnrollmentAPI.Models;
using Xunit;

namespace StudentEnrollmentApi.Tests
{
    public class StudentEnrollmentServiceTests : IDisposable
    {
        private readonly EnrollmentContext _context;

        public StudentEnrollmentServiceTests()
        {
            var options = new DbContextOptionsBuilder<EnrollmentContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new EnrollmentContext(options);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public void AddStudent_ShouldPersistStudent()
        {
            // Arrange
            var student = new Students
            {
                FirstName = "Mike",
                LastName = "Amparo",
                Year = 1,
                Gender = "F",
                Enrolled = true
            };

            // Act
            _context.Students.Add(student);
            _context.SaveChanges();

            // Assert
            var savedStudent = _context.Students.FirstOrDefault(s => s.FirstName == "Mike" && s.LastName == "Amparo");
            Assert.NotNull(savedStudent);
        }

        [Fact]
        public void AddProgram_ShouldPersistProgram()
        {
            // Arrange
            var program = new Programs
            {
                ProgramName = "ITT-101"
            };

            // Act
            _context.Programs.Add(program);
            _context.SaveChanges();

            // Assert
            var savedProgram = _context.Programs.FirstOrDefault(p => p.ProgramName == "ITT-101");
            Assert.NotNull(savedProgram);
        }

        [Fact]
        public void AddSection_ShouldPersistSection()
        {   
            // Arrange
            var section = new Sections
            {
                Code = "asdas",
                Year = 1,
                ProgramId = 1
            };

            // Act
            _context.Sections.Add(section);
            _context.SaveChanges();

            // Assert 
            var savedSection = _context.Sections.FirstOrDefault(sec => sec.Code == "asdas" && sec.Year == 1 && sec.ProgramId == 1  );
            Assert.NotNull(savedSection);
        }   

        [Fact]
        public void AddEnrollment_ShouldPersistEnrollment()
        {
            // Arrange
            var enrollment = new StudentGrades
            {
                StudentId = 1,
                Student_SectionsId = 1,
                grade = 90
            };

            // Act
            _context.StudentGrades.Add(enrollment);
            _context.SaveChanges();

            // Assert
            var savedEnrollment = _context.StudentGrades.FirstOrDefault(e => e.StudentId == 1 && e.Student_SectionsId == 1);
            Assert.NotNull(savedEnrollment);
        }

        [Fact]
        public void GetStudentEnrollments_ShouldReturnEnrollments()
        {
            // Arrange
            var studentId = 1;
            var sectionId = 1;

            // Act
            var enrollments = _context.StudentGrades.Where(e => e.StudentId == studentId && e.Student_SectionsId == sectionId).ToList();

            // Assert
            Assert.NotEmpty(enrollments);
        }
    }
}
