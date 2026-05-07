namespace Day3WebApi.Controllers
{
    using Day3WebApi.DTOs;
    using Day3WebApi.Models;
    using Day3WebApi.Services;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBooksService booksService;

        public BooksController(IBooksService _booksService)
        {
            booksService = _booksService;
        }

        // GET /api/books
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(booksService.GetAll());
        }


        // GET /api/books/1
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            return Ok(booksService.GetById(id));
        }

        // GET /api/books/search?genre=Fiction&available=true
        [HttpGet("search")]
        public IActionResult Search(
            [FromQuery] string? genre,
            [FromQuery] bool? available)
        {
            throw new Exception();
            return Ok(booksService.Search(genre, available));
         }

            // GET /api/books/stats
            [HttpGet("stats")]
        public IActionResult GetStats()
        {
            return Ok(booksService.GetStats());
        }

        // POST /api/books
        [HttpPost]
        public IActionResult Create([FromBody] Book book)
        {
            booksService.Create(book);
            return Ok();
        }

        // PUT /api/books/1
        [HttpPut("{id:int}")]
        public IActionResult FullUpdate(int id, [FromBody] Book updated)
        {
            booksService.FullUpdate(id, updated);
            return Ok();
        }

        // PATCH /api/books/1
        [HttpPatch("{id:int}")]
        public IActionResult PartialUpdate(int id, [FromBody] BookPatchDto patch)
        {
            booksService.PartialUpdate(id, patch);
            return Ok();
        }

        // DELETE /api/books/1
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            booksService.Delete(id);
            return NoContent();
        }
    }

}
