using LibraryApp.Domain;
using LibraryApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Api.Controllers
{
    public class LibraryController : ControllerBase
    {
        // OLD approach -> private static readonly LibraryService _service = new LibraryService();
        private readonly ILibraryService _service;
        public LibraryController(ILibraryService libraryService)
        {
            _service = libraryService;
        }
        // Add GET to list all library items
        [HttpGet("items")]
        public IActionResult GetItems()
        {
            _service.Seed();
            var items = _service.Items;
            return Ok(items);
        }

        [HttpPost("books")]
        public IActionResult AddBook([FromBody] Book book)
        {
            if (book == null || string.IsNullOrWhiteSpace(book.Title) || string.IsNullOrWhiteSpace(book.Author))
            {
                return BadRequest("Invalid book data.");
            }
            var addedBook = _service.AddBook(book.Title, book.Author, book.Pages);
            return CreatedAtAction(nameof(GetItems), new { id = addedBook.Id }, addedBook);
        }

        // TODO: Add more endpoints for magazines, members, borrowing, and returning items
    }
}