using EnrollmentSystem.Models;
using EnrollmentSystem.Services.Students;
using Microsoft.AspNetCore.Mvc;

namespace EnrollmentSystem.Controllers;

[ApiController]
[Route("api/section/{sectionId}/student")]
public class StudentController : ControllerBase
{
	private readonly IStudentServices _studentServices;

	public StudentController(IStudentServices studentServices)
	{
		_studentServices = studentServices;
	}

	[HttpGet]
	[HttpGet]
	public ActionResult<List<Student>> GetAll(int sectionId)
	{
		var students = _studentServices.Search(null, null, null, sectionId, null);

		foreach (var student in students)
		{
			if (Helpers.GradeStore.Grades.ContainsKey(student.Id))
			{
				student.Grade = Helpers.GradeStore.Grades[student.Id];
			}
		}

		return Ok(students);
	}

	[HttpGet("{studentId:int}")]
	public IActionResult GetById(int sectionId, int studentId)
	{
		var student = _studentServices.GetById(studentId);

		if (student == null || student.SectionId != sectionId)
			return NotFound();

		if (Helpers.GradeStore.Grades.ContainsKey(student.Id))
		{
			student.Grade = Helpers.GradeStore.Grades[student.Id];
		}

		return Ok(student);
	}

	[HttpGet("search")]
	public ActionResult<List<Student>> Search(
		int sectionId,
		[FromQuery] string? firstName,
		[FromQuery] string? lastName,
		[FromQuery] int? age,
		[FromQuery] char? gender)
	{
		var students = _studentServices.Search(firstName, lastName, age, sectionId, gender);
		return Ok(students);
	}

	[HttpPost]
	public IActionResult Create(int sectionId, [FromBody] Student student)
	{
		student.SectionId = sectionId;
		_studentServices.Create(student);

		if (HttpContext.Items.ContainsKey("GeneratedGrade"))
		{
			student.Grade = (int)HttpContext.Items["GeneratedGrade"]!;
		}
		return CreatedAtAction(nameof(GetById), new { sectionId = sectionId, studentId = student.Id }, student);
	}

	[HttpPut("{studentId:int}")]
	public IActionResult Update(int sectionId, int studentId, [FromBody] Student updated)
	{
		var existing = _studentServices.GetById(studentId);
		if (existing == null || existing.SectionId != sectionId) return NotFound();
		updated.SectionId = sectionId;
		_studentServices.Update(studentId, updated);
		return NoContent();
	}

	[HttpPatch("{studentId:int}")]
	public IActionResult Patch(int sectionId, int studentId, [FromBody] Student patched)
	{
		var existing = _studentServices.GetById(studentId);
		if (existing == null || existing.SectionId != sectionId) return NotFound();
		patched.SectionId = sectionId;
		_studentServices.Patch(studentId, patched);
		return NoContent();
	}

	[HttpDelete("{studentId:int}")]
	public IActionResult Delete(int sectionId, int studentId)
	{
		var existing = _studentServices.GetById(studentId);
		if (existing == null || existing.SectionId != sectionId) return NotFound();
		_studentServices.Delete(studentId);
		return NoContent();
	}
}
