using EnrollmentSystem.Models;
using EnrollmentSystem.Services.Student;
using Microsoft.AspNetCore.Mvc;

namespace EnrollmentSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
	private readonly IStudentServices studentServices;

	public StudentController(IStudentServices _studentServices)
	{
		studentServices = _studentServices;
	}

	[HttpGet]
	public ActionResult<List<Student>> GetAll()
	{
		return Ok(_studentServices.GetAll());
	}

	[HttpGet("{id:int}")]
	public IActionResult GetById(int id)
	{
		return Ok(studentServices.GetById(id));
	}

	[HttpGet("search")]
	public ActionResult<List<Student>> Search(
		[FromQuery] string? firstName,
		[FromQuery] string? lastName,
		[FromQuery] int? age,
		[FromQuery] int? sectionId,
		[FromQuery] char? gender)
	{
		return Ok(_studentServices.Search(firstName, lastName, age, sectionId, gender));
	}

	[HttpPost]
	public IActionResult Create([FromBody] Student student)
	{
		studentServices.Create(student);
		return Ok();
	}

	[HttpPut("{id:int}")]
	public IActionResult Update(int id, [FromBody] Student updated)
	{
		studentServices.Update(id, updated);
		return Ok();
	}

	[HttpPatch("{id:int}")]
	public IActionResult Patch(int id, [FromBody] Student patched)
	{
		studentServices.Patch(id, patched);
		return Ok();
	}

	[HttpDelete("{id:int}")]
	public IActionResult Delete(int id)
	{
		studentServices.Delete(id);
		return Ok();
	}
}
