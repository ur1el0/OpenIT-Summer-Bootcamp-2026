using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentAPI;
using StudentEnrollmentAPI.Models;

namespace StudentEnrollmentAPI.Controllers;

[ApiController]
[Route("api/student-sections")]
public class StudentSectionsController : ControllerBase
{
    private readonly EnrollmentContext _context;

    public StudentSectionsController(EnrollmentContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Student_Sections>>> GetAll()
    {
        var studentSections = await _context.Student_Sections
            .Include(item => item.Student)
            .Include(item => item.Section)
            .ToListAsync();

        return Ok(studentSections);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Student_Sections>> GetById(int id)
    {
        var studentSection = await _context.Student_Sections
            .Include(item => item.Student)
            .Include(item => item.Section)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (studentSection is null)
        {
            return NotFound();
        }

        return Ok(studentSection);
    }

    [HttpPost]
    public async Task<ActionResult<Student_Sections>> Create(Student_Sections studentSection)
    {
        _context.Student_Sections.Add(studentSection);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = studentSection.Id }, studentSection);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Student_Sections studentSection)
    {
        var existingStudentSection = await _context.Student_Sections.FindAsync(id);
        if (existingStudentSection is null)
        {
            return NotFound();
        }

        existingStudentSection.StudentId = studentSection.StudentId;
        existingStudentSection.SectionId = studentSection.SectionId;
        existingStudentSection.EnrolledAt = studentSection.EnrolledAt;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var studentSection = await _context.Student_Sections.FindAsync(id);
        if (studentSection is null)
        {
            return NotFound();
        }

        _context.Student_Sections.Remove(studentSection);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}