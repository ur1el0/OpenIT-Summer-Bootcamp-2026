using Microsoft.AspNetCore.Mvc;
using LibraryApi.Models;

namespace LibraryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private static List<Book> _books = new()
    {
        new Book { Id = 1, Title = "Clean Code",               Author = "Robert Martin", Genre = "Programming", Year = 2008, Available = true  },
        new Book { Id = 2, Title = "C# in Depth",              Author = "Jon Skeet",     Genre = "Programming", Year = 2019, Available = false },
        new Book { Id = 3, Title = "Dune",                     Author = "Frank Herbert", Genre = "Fiction",      Year = 1965, Available = true  },
        new Book { Id = 4, Title = "The Pragmatic Programmer",  Author = "David Thomas",  Genre = "Programming", Year = 1999, Available = false },
        new Book { Id = 5, Title = "Sapiens",                   Author = "Yuval Harari",  Genre = "History",     Year = 2011, Available = true  },
    };

    // GET /api/books
    [HttpGet]
    public IActionResult GetAll() => Ok(_books);

    // GET /api/books/1
    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);
        return book == null ? NotFound() : Ok(book);
    }

    // GET /api/books/search?genre=Fiction&available=true
    [HttpGet("search")]
    public IActionResult Search(
        [FromQuery] string? genre,
        [FromQuery] bool?   available,
        [FromQuery] string? author)
    {
        var result = _books.AsEnumerable();
        if (!string.IsNullOrEmpty(genre))
            result = result.Where(b => b.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase));
        if (available.HasValue)
            result = result.Where(b => b.Available == available.Value);
        if (!string.IsNullOrEmpty(author))
            result = result.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase));
        return Ok(result.ToList());
    }

    // GET /api/books/stats
    [HttpGet("stats")]
    public IActionResult GetStats()
    {
        var stats = new
        {
            Total     = _books.Count,
            Available = _books.Count(b => b.Available),
            Borrowed  = _books.Count(b => !b.Available),
            ByGenre   = _books
                .GroupBy(b => b.Genre)
                .Select(g => new { Genre = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .ToList()
        };
        return Ok(stats);
    }

    // POST /api/books
    [HttpPost]
    public IActionResult Create([FromBody] Book book)
    {
        book.Id = _books.Count == 0 ? 1 : _books.Max(b => b.Id) + 1;
        _books.Add(book);
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    // PUT /api/books/1
    [HttpPut("{id:int}")]
    public IActionResult FullUpdate(int id, [FromBody] Book updated)
    {
        var existing = _books.FirstOrDefault(b => b.Id == id);
        if (existing == null) return NotFound();
        existing.Title     = updated.Title;
        existing.Author    = updated.Author;
        existing.Genre     = updated.Genre;
        existing.Year      = updated.Year;
        existing.Available = updated.Available;
        return Ok(existing);
    }

    // PATCH /api/books/1
    [HttpPatch("{id:int}")]
    public IActionResult PartialUpdate(int id, [FromBody] BookPatchDto patch)
    {
        var existing = _books.FirstOrDefault(b => b.Id == id);
        if (existing == null) return NotFound();
        if (patch.Title     != null) existing.Title     = patch.Title;
        if (patch.Available != null) existing.Available = patch.Available.Value;
        return Ok(existing);
    }

    // DELETE /api/books/1
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);
        if (book == null) return NotFound();
        _books.Remove(book);
        return NoContent();
    }
}