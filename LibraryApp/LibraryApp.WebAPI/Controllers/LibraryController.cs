using LibraryApp.Console.Domain;
using LibraryApp.Console.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.WebAPI.Controllers
{
    public class LibraryController : ControllerBase
    {
        private readonly ILibraryService _libraryService;

        public LibraryController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        [HttpGet("items")]
        public IActionResult GetItems()
        {
            _libraryService.Seed(); // Seed data for demonstration purposes
            var items = _libraryService.Items;
            return Ok(items);
        }

        [HttpPost("book")]
        public IActionResult AddBook([FromBody] Book book)
        { 
            if (book == null || string.IsNullOrWhiteSpace(book.Title) || string.IsNullOrWhiteSpace(book.Author))
            {
                return BadRequest("Invalid book data.");
            }
            var addedBook = _libraryService.AddBook(book.Title, book.Author, book.Pages);
            return CreatedAtAction(nameof(GetItems), new { id = addedBook.Id }, addedBook);
        }
    }
}