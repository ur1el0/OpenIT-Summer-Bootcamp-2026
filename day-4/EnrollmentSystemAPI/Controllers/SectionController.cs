using EnrollmentSystemApi.Services.Students;
using EnrollmentSystemApi.Services.Sections;
using EnrollmentSystemApi.DTOs.Sections;
using Microsoft.AspNetCore.Mvc;

namespace EnrollmentSystemApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SectionController : ControllerBase
{
	private readonly ISectionService _sectionsService;
	private readonly IStudentService _studentServices;

	public SectionController(ISectionService sectionsService, IStudentService studentServices)
	{
		_sectionsService = sectionsService;
		_studentServices = studentServices;
	}

	[HttpGet]
	public ActionResult<List<SectionResponseDTO>> GetAll()
	{
		return Ok(_sectionsService.GetAllSections());
	}

	[HttpGet("{sectionId:int}/student/{studentId:int}")]
	public IActionResult GetStudentInSection(int sectionId, int studentId)
	{
		var section = _sectionsService.GetSectionById(sectionId);
		if (section == null)
		{
			return NotFound($"Section {sectionId} not found.");
		}

		var student = _studentServices.GetStudentBySectionCodeAndId(section.Code, studentId);
		if (student == null)
		{
			return NotFound($"Student {studentId} not found in section {sectionId}.");
		}

		return Ok(student);
	}
}
