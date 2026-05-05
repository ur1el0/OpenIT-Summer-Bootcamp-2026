using Microsoft.AspNetCore.Mvc;
using StudentEnrollmentAPI.Model;
using StudentEnrollmentAPI.Model.Dto;

namespace StudentEnrollmentAPI.Controllers;

[ApiController]
[Route("api/[controller]")]

public class StudentController : ControllerBase
{
    private static List<Student> students = new()
    {
        new Student(1, "Roosc", "Zaño", 3, "Computer Science", "Male", "Dalahican National High School", "Yes", "Computer Science", "Lucena City"),
        new Student(2, "Mike", "Gomez", 4, "Information Technology", "Female", "Talipan National High School", "Yes", "IT", "Lucena City"),
        new Student(3, "Kurt", "Laja", 2, "Computer Science", "Male", "ALS High School", "Yes", "Computer Science", "Lucena City"),
        new Student(4, "Ron", "Cada", 1, "Engineering", "Female", "FEU Tech", "Yes", "Engineering", "Lucena City")
    };

    // In-memory list of old schools derived from sample students
    private static List<string> oldSchools = students.Select(s => s.OldSchool).Distinct().ToList();

    [HttpGet]
    public ActionResult<IEnumerable<Student>> GetAll()
    {
        return Ok(students);
    }

    [HttpGet("{id}")]
    public ActionResult<Student> GetById(int id)
    {
        var student = students.FirstOrDefault(s => s.Id == id);
        if (student == null)
        {
            return NotFound();
        }
        return Ok(student);
    }

    [HttpGet("search")]
    public ActionResult<IEnumerable<Student>> Search(
        [FromQuery] string? gender = null,
        [FromQuery] string? course = null,
        [FromQuery] int? year = null,
        [FromQuery] string? department = null)
    {
        var query = students.AsEnumerable();

        if (!string.IsNullOrEmpty(gender))
        {
            query = query.Where(s => s.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(course))
        {
            query = query.Where(s => s.Course.Contains(course, StringComparison.OrdinalIgnoreCase));
        }

        if (year.HasValue)
        {
            query = query.Where(s => s.Year == year);
        }

        if (!string.IsNullOrEmpty(department))
        {
            query = query.Where(s => s.Department.Contains(department, StringComparison.OrdinalIgnoreCase));
        }

        return Ok(query.ToList());
    }

    [HttpGet("stats")]
    public ActionResult GetStats()
    {
        var byDepartment = students
            .GroupBy(s => s.Department)
            .Select(g => new { Department = g.Key, Count = g.Count() })
            .ToList();

        var byGender = students
            .GroupBy(s => s.Gender)
            .Select(g => new { Gender = g.Key, Count = g.Count() })
            .ToList();

        return Ok(new
        {
            Total = students.Count,
            ByDepartment = byDepartment,
            ByGender = byGender,
            OldSchools = oldSchools
        });
    }

    [HttpPost]
    public ActionResult<Student> Create([FromBody] CreateStudentDto dto)
    {
        var maxId = students.Max(s => s.Id);
        var newStudent = new Student(
            maxId + 1,
            dto.FirstName,
            dto.LastName,
            dto.Year,
            dto.Course,
            dto.Gender,
            dto.OldSchool,
            dto.Enrolled,
            dto.Department,
            dto.Address
        );
        students.Add(newStudent);
        return CreatedAtAction(nameof(GetById), new { id = newStudent.Id }, newStudent);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] UpdateStudentDto dto)
    {
        var student = students.FirstOrDefault(s => s.Id == id);
        if (student == null)
        {
            return NotFound();
        }

        var updated = new Student(
            student.Id,
            dto.FirstName ?? student.FirstName,
            dto.LastName ?? student.LastName,
            dto.Year ?? student.Year,
            dto.Course ?? student.Course,
            dto.Gender ?? student.Gender,
            dto.OldSchool ?? student.OldSchool,
            dto.Enrolled ?? student.Enrolled,
            dto.Department ?? student.Department,
            dto.Address ?? student.Address
        );

        var index = students.IndexOf(student);
        students[index] = updated;
        return Ok(updated);
    }

    [HttpPatch("{id}")]
    public IActionResult PartialUpdate(int id, [FromBody] UpdateStudentDto dto)
    {
        var student = students.FirstOrDefault(s => s.Id == id);
        if (student == null)
        {
            return NotFound();
        }

        var updated = new Student(
            student.Id,
            dto.FirstName ?? student.FirstName,
            dto.LastName ?? student.LastName,
            dto.Year ?? student.Year,
            dto.Course ?? student.Course,
            dto.Gender ?? student.Gender,
            dto.OldSchool ?? student.OldSchool,
            dto.Enrolled ?? student.Enrolled,
            dto.Department ?? student.Department,
            dto.Address ?? student.Address
        );

        var index = students.IndexOf(student);
        students[index] = updated;
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var student = students.FirstOrDefault(s => s.Id == id);
        if (student == null)
        {
            return NotFound();
        }

        students.Remove(student);
        return NoContent();
    }
}