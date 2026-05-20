using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
	[AllowAnonymous]
	public async Task<ActionResult<IEnumerable<StudentSections>>> GetAll()
	{
		var studentSections = await _context.Student_Sections.ToListAsync();
		return Ok(studentSections);
	}

	[HttpGet("{id:int}")]
	[AllowAnonymous]
	public async Task<ActionResult<StudentSections>> GetById(int id)
	{
		var studentSection = await _context.Student_Sections.FindAsync(id);
		if (studentSection is null)
		{
			return NotFound();
		}

		return Ok(studentSection);
	}

	[HttpPost]
	[Authorize]
	public async Task<ActionResult<StudentSections>> Create(StudentSections studentSection)
	{
		_context.Student_Sections.Add(studentSection);
		await _context.SaveChangesAsync();

		return CreatedAtAction(nameof(GetById), new { id = studentSection.Id }, studentSection);
	}

	[HttpPut("{id:int}")]
	[Authorize]
	public async Task<IActionResult> Update(int id, StudentSections studentSection)
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
	[Authorize]
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
