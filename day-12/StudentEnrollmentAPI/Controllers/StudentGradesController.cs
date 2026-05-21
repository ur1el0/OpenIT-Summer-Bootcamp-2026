using EnrollmentSystem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
	[AllowAnonymous]
	public async Task<ActionResult<IEnumerable<StudentGrades>>> GetAll()
	{
		var studentGrades = await _context.StudentGrades.ToListAsync();
		return Ok(studentGrades);
	}

	[HttpGet("{id:int}")]
	[AllowAnonymous]
	public async Task<ActionResult<StudentGrades>> GetById(int id)
	{
		var studentGrade = await _context.StudentGrades.FindAsync(id);
		if (studentGrade is null)
		{
			return NotFound();
		}

		return Ok(studentGrade);
	}

	[HttpPost]
	[Authorize]
	public async Task<ActionResult<StudentGrades>> Create(StudentGrades studentGrade)
	{
		_context.StudentGrades.Add(studentGrade);
		await _context.SaveChangesAsync();

		return CreatedAtAction(nameof(GetById), new { id = studentGrade.Id }, studentGrade);
	}

	[HttpPut("{id:int}")]
	[Authorize]
	public async Task<IActionResult> Update(int id, StudentGrades studentGrade)
	{
		var existingStudentGrade = await _context.StudentGrades.FindAsync(id);
		if (existingStudentGrade is null)
		{
			return NotFound();
		}

		existingStudentGrade.StudentId = studentGrade.StudentId;
		existingStudentGrade.Student_SectionsId = studentGrade.Student_SectionsId;
		existingStudentGrade.grade = studentGrade.grade;

		await _context.SaveChangesAsync();

		return NoContent();
	}

	[HttpDelete("{id:int}")]
	[Authorize]
	public async Task<IActionResult> Delete(int id)
	{
		var studentGrade = await _context.StudentGrades.FindAsync(id);
		if (studentGrade is null)
		{
			return NotFound();
		}

		_context.StudentGrades.Remove(studentGrade);
		await _context.SaveChangesAsync();

		return NoContent();
	}
}
