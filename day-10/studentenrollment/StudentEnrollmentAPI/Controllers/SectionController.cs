using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentAPI;
using StudentEnrollmentAPI.Models;

namespace StudentEnrollmentAPI.Controllers;

[ApiController]
[Route("api/sections")]
public class SectionController : ControllerBase
{
	private readonly EnrollmentContext _context;

	public SectionController(EnrollmentContext context)
	{
		_context = context;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<Sections>>> GetAll()
	{
		var sections = await _context.Sections.ToListAsync();
		return Ok(sections);
	}

	[HttpGet("{id:int}")]
	public async Task<ActionResult<Sections>> GetById(int id)
	{
		var section = await _context.Sections.FindAsync(id);
		if (section is null)
		{
			return NotFound();
		}

		return Ok(section);
	}

	[HttpPost]
	public async Task<ActionResult<Sections>> Create(Sections section)
	{
		_context.Sections.Add(section);
		await _context.SaveChangesAsync();

		return CreatedAtAction(nameof(GetById), new { id = section.Id }, section);
	}

	[HttpPut("{id:int}")]
	public async Task<IActionResult> Update(int id, Sections section)
	{
		var existingSection = await _context.Sections.FindAsync(id);
		if (existingSection is null)
		{
			return NotFound();
		}

		existingSection.Code = section.Code;
		existingSection.Year = section.Year;
		existingSection.ProgramId = section.ProgramId;

		await _context.SaveChangesAsync();

		return NoContent();
	}

	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		var section = await _context.Sections.FindAsync(id);
		if (section is null)
		{
			return NotFound();
		}

		_context.Sections.Remove(section);
		await _context.SaveChangesAsync();

		return NoContent();
	}
}
