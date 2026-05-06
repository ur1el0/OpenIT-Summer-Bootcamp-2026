using Day3WebApi.Models;
using Day3WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Day3WebApi.Controllers
{
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        public readonly IBooksService BooksService;
        public AuthorsController(IBooksService booksService)
        {
            BooksService = booksService;
        }


        [HttpGet("api/books/{id:int}/authors")]
        public IActionResult GetAll(int id)
        {
            var book = BooksService.GetById(id);
            return Ok(book.Authors);
        }
    }
}
