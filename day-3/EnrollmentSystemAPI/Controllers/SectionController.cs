using EnrollmentSystem.Models;
using EnrollmentSystem.Services.Sections;
using Microsoft.AspNetCore.Mvc;

namespace EnrollmentSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SectionController : ControllerBase
{
	private readonly ISectionsService _sectionsService;

	public SectionController(ISectionsService sectionsService)
	{
		_sectionsService = sectionsService;
	}

	[HttpGet]
	public ActionResult<List<Section>> GetAll()
	{
		return Ok(_sectionsService.GetAll());
	}
}
