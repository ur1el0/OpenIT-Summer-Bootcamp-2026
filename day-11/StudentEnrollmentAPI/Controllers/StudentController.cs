using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using StudentEnrollmentAPI;
using StudentEnrollmentAPI.Models;

namespace StudentEnrollmentAPI.Controllers;

[ApiController]
[Route("api/students")]
public class StudentController : ControllerBase
{
	private readonly EnrollmentContext _context;

	public StudentController(EnrollmentContext context)
	{
		_context = context;
	}

	[HttpGet]
	[AllowAnonymous]
	public async Task<ActionResult<IEnumerable<Students>>> GetAll()
	{
		var students = await _context.Students.ToListAsync();
		return Ok(students);
	}

	[HttpGet("{id:int}")]
	[AllowAnonymous]
	public async Task<ActionResult<Students>> GetById(int id)
	{
		var student = await _context.Students.FindAsync(id);
		if (student is null)
		{
			return NotFound();
		}

		return Ok(student);
	}

	[HttpPost]
	[Authorize]
	public async Task<ActionResult<Students>> Create(Students student)
	{
		_context.Students.Add(student);
		await _context.SaveChangesAsync();

		return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
	}

	[HttpPut("{id:int}")]
	[Authorize]
	public async Task<IActionResult> Update(int id, Students student)
	{
		var existingStudent = await _context.Students.FindAsync(id);
		if (existingStudent is null)
		{
			return NotFound();
		}

		existingStudent.FirstName = student.FirstName;
		existingStudent.LastName = student.LastName;
		existingStudent.Year = student.Year;
		existingStudent.Gender = student.Gender;
		existingStudent.Enrolled = student.Enrolled;

		await _context.SaveChangesAsync();

		return NoContent();
	}

	[HttpDelete("{id:int}")]
	[Authorize]
	public async Task<IActionResult> Delete(int id)
	{
		var student = await _context.Students.FindAsync(id);
		if (student is null)
		{
			return NotFound();
		}

		_context.Students.Remove(student);
		await _context.SaveChangesAsync();

		return NoContent();
	}
}
