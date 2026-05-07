using Microsoft.AspNetCore.Mvc;
using EnrollmentSystemApi.DTOs.Sections;
using EnrollmentSystemApi.DTOs.Students;
using EnrollmentSystemApi.Services.Sections;
using EnrollmentSystemApi.Services.Students;

namespace EnrollmentSystemApi.controllers;

[ApiController]
[Route("api/students")]
public class StudentController(IStudentService studentService, ISectionService sectionService) : ControllerBase
{
	[HttpGet("~/api/sections")]
	public ActionResult<List<SectionResponseDTO>> GetAllSections()
	{
		return Ok(sectionService.GetAllSections());
	}

	[HttpGet("~/api/sections/{id:int}")]
	public ActionResult<SectionResponseDTO> GetSectionById(int id)
	{
		var section = sectionService.GetSectionById(id);
		if (section is null)
		{
			return NotFound();
		}

		return Ok(section);
	}

	[HttpGet("~/api/sections/{sectionCode}")]
	public ActionResult<SectionResponseDTO> GetSectionByCode(string sectionCode)
	{
		var section = sectionService.GetSectionByCode(sectionCode);
		if (section is null)
		{
			return NotFound();
		}

		return Ok(section);
	}

	[HttpGet]
	public ActionResult<List<StudentResponseDTO>> GetAll(
		[FromQuery] string? firstName,
		[FromQuery] string? lastName,
		[FromQuery] string? gender,
		[FromQuery] int? age)
	{
		return Ok(studentService.GetStudents(firstName, lastName, gender, age));
	}

	[HttpGet("~/api/sections/{sectionCode}/students")]
	public ActionResult<List<StudentResponseDTO>> GetStudentsBySectionCode(
		string sectionCode,
		[FromQuery] string? firstName,
		[FromQuery] string? lastName,
		[FromQuery] string? gender,
		[FromQuery] int? age)
	{
		var students = studentService.GetStudentsBySectionCode(sectionCode, firstName, lastName, gender, age);
		if (students is null)
		{
			return NotFound();
		}

		return Ok(students);
	}

	[HttpGet("{id:int}")]
	public ActionResult<StudentResponseDTO> GetById(int id)
	{
		var student = studentService.GetStudentById(id);
		if (student is null)
		{
			return NotFound();
		}

		return Ok(student);
	}

	[HttpGet("~/api/sections/{sectionCode}/students/{studentId:int}")]
	public ActionResult<StudentResponseDTO> GetStudentBySectionAndId(string sectionCode, int studentId)
	{
		var student = studentService.GetStudentBySectionCodeAndId(sectionCode, studentId);
		if (student is null)
		{
			return NotFound();
		}

		return Ok(student);
	}

	[HttpPost]
	public ActionResult<StudentResponseDTO> Create([FromBody] StudentCreateDTO studentCreateDTO)
	{
		var createdStudent = studentService.Create(studentCreateDTO);
		if (createdStudent is null)
		{
			return BadRequest("SectionId does not exist.");
		}

		return CreatedAtAction(nameof(GetById), new { id = createdStudent.Id }, createdStudent);
	}

	[HttpPost("~/api/sections/{sectionCode}/students")]
	public ActionResult<StudentResponseDTO> CreateInSection(string sectionCode, [FromBody] StudentCreateDTO studentCreateDTO)
	{
		var createdStudent = studentService.CreateInSection(sectionCode, studentCreateDTO);
		if (createdStudent is null)
		{
			return NotFound();
		}

		return CreatedAtAction(
			nameof(GetStudentBySectionAndId),
			new { sectionCode, studentId = createdStudent.Id },
			createdStudent);
	}

	[HttpPut("{id:int}")]
	public IActionResult Update(int id, [FromBody] StudentUpdateDTO studentUpdateDTO)
	{
		var updated = studentService.Update(id, studentUpdateDTO);
		return updated ? NoContent() : NotFound();
	}

	[HttpPut("~/api/sections/{sectionCode}/students/{studentId:int}")]
	public IActionResult UpdateInSection(string sectionCode, int studentId, [FromBody] StudentUpdateDTO studentUpdateDTO)
	{
		var updated = studentService.UpdateInSection(sectionCode, studentId, studentUpdateDTO);
		return updated ? NoContent() : NotFound();
	}

	[HttpPatch("{id:int}")]
	public IActionResult Patch(int id, [FromBody] StudentPatchDTO studentPatchDTO)
	{
		var patched = studentService.Patch(id, studentPatchDTO);
		return patched ? NoContent() : NotFound();
	}

	[HttpPatch("~/api/sections/{sectionCode}/students/{studentId:int}")]
	public IActionResult PatchInSection(string sectionCode, int studentId, [FromBody] StudentPatchDTO studentPatchDTO)
	{
		var patched = studentService.PatchInSection(sectionCode, studentId, studentPatchDTO);
		return patched ? NoContent() : NotFound();
	}

	[HttpDelete("{id:int}")]
	public IActionResult Delete(int id)
	{
		var deleted = studentService.Delete(id);
		return deleted ? NoContent() : NotFound();
	}

	[HttpDelete("~/api/sections/{sectionCode}/students/{studentId:int}")]
	public IActionResult DeleteInSection(string sectionCode, int studentId)
	{
		var deleted = studentService.DeleteInSection(sectionCode, studentId);
		return deleted ? NoContent() : NotFound();
	}

	[HttpPost("~/api/sections")]
	public ActionResult<SectionResponseDTO> CreateSection([FromBody] SectionCreateDTO sectionCreateDTO)
	{
		var createdSection = sectionService.Create(sectionCreateDTO);
		return CreatedAtAction(nameof(GetSectionById), new { id = createdSection.Id }, createdSection);
	}

	[HttpPut("~/api/sections/{id:int}")]
	public IActionResult UpdateSection(int id, [FromBody] SectionUpdateDTO sectionUpdateDTO)
	{
		var updated = sectionService.Update(id, sectionUpdateDTO);
		return updated ? NoContent() : NotFound();
	}

	[HttpPatch("~/api/sections/{id:int}")]
	public IActionResult PatchSection(int id, [FromBody] SectionPatchDTO sectionPatchDTO)
	{
		var patched = sectionService.Patch(id, sectionPatchDTO);
		return patched ? NoContent() : NotFound();
	}

	[HttpDelete("~/api/sections/{id:int}")]
	public IActionResult DeleteSection(int id)
	{
		var deleted = sectionService.Delete(id);
		return deleted ? NoContent() : NotFound();
	}
}

