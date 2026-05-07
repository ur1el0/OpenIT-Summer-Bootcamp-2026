using EnrollmentSystem.Models;
using EnrollmentSystem.Services.Students;
using EnrollmentSystem.Services.Sections;
using Microsoft.AspNetCore.Mvc;

namespace EnrollmentSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SectionController : ControllerBase
{
	private readonly ISectionsService _sectionsService;
	private readonly IStudentServices _studentServices;

	public SectionController(ISectionsService sectionsService, IStudentServices studentServices)
	{
		_sectionsService = sectionsService;
		_studentServices = studentServices;
	}

	[HttpGet]
	public ActionResult<List<Section>> GetAll()
	{
		return Ok(_sectionsService.GetAll());
	}

	[HttpGet("{sectionId:int}/student/{studentId:int}")]
	public IActionResult GetStudentInSection(int sectionId, int studentId)
	{
		var section = _sectionsService.GetAll().FirstOrDefault(s => s.Id == sectionId);
		if (section == null)
		{
			return NotFound($"Section {sectionId} not found.");
		}

		var student = _studentServices.GetById(studentId);
		if (student == null || student.SectionId != sectionId)
		{
			return NotFound($"Student {studentId} not found in section {sectionId}.");
		}

		return Ok(student);
	}
}
