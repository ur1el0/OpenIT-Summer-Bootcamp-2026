using EnrollmentSystem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentAPI;

namespace StudentEnrollmentAPI.Controllers;

[ApiController]
[Route("api/student-grades")]
public class StudentGradesController : ControllerBase
{
    private readonly EnrollmentContext _context;

    public StudentGradesController(EnrollmentContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentGrades>>> GetAll()
    {
        var grades = await _context.StudentGrades
            .Include(item => item.Student)
            .Include(item => item.Section)
            .Include(item => item.Student_Sections)
            .ToListAsync();

        return Ok(grades);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<StudentGrades>> GetById(int id)
    {
        var grade = await _context.StudentGrades
            .Include(item => item.Student)
            .Include(item => item.Section)
            .Include(item => item.Student_Sections)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (grade is null)
        {
            return NotFound();
        }

        return Ok(grade);
    }

    [HttpPost]
    public async Task<ActionResult<StudentGrades>> Create(StudentGrades grade)
    {
        _context.StudentGrades.Add(grade);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = grade.Id }, grade);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, StudentGrades grade)
    {
        var existingGrade = await _context.StudentGrades.FindAsync(id);
        if (existingGrade is null)
        {
            return NotFound();
        }

        existingGrade.StudentId = grade.StudentId;
        existingGrade.Student_SectionsId = grade.Student_SectionsId;
        existingGrade.grade = grade.grade;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var grade = await _context.StudentGrades.FindAsync(id);
        if (grade is null)
        {
            return NotFound();
        }

        _context.StudentGrades.Remove(grade);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}