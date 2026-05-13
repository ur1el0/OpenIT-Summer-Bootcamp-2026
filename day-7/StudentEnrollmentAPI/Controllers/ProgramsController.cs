using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentAPI;
using StudentEnrollmentAPI.Models;

namespace StudentEnrollmentAPI.Controllers;

[ApiController]
[Route("api/programs")]
public class ProgramsController : ControllerBase
{
    private readonly EnrollmentContext _context;

    public ProgramsController(EnrollmentContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Programs>>> GetAll()
    {
        var programs = await _context.Programs.ToListAsync();
        return Ok(programs);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Programs>> GetById(int id)
    {
        var program = await _context.Programs.FindAsync(id);
        if (program is null)
        {
            return NotFound();
        }

        return Ok(program);
    }

    [HttpPost]
    public async Task<ActionResult<Programs>> Create(Programs program)
    {
        _context.Programs.Add(program);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = program.Id }, program);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Programs program)
    {
        var existingProgram = await _context.Programs.FindAsync(id);
        if (existingProgram is null)
        {
            return NotFound();
        }

        existingProgram.ProgramName = program.ProgramName;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var program = await _context.Programs.FindAsync(id);
        if (program is null)
        {
            return NotFound();
        }

        _context.Programs.Remove(program);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}