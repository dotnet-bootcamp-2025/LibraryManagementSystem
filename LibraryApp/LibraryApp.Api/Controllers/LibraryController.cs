using Microsoft.AspNetCore.Mvc;
//using LibraryApp.Console.Services;
//using LibraryApp.Console.Domain;
using LibraryApp.Domain;
using LibraryApp.Services;

namespace LibraryApp.Api.Controllers
{
    public class LibraryController : ControllerBase
    {
        //OLD private static readonly LibraryService _service = new();
        private readonly ILibraryService _service;

        public LibraryController(ILibraryService LibraryService )
        {
            _service = LibraryService;
        }

        //Add GET to list all library items
        [HttpGet("items")]
        public IActionResult GetItems()
        {
            _service.Seed();
            var items = _service.Items;
            return Ok(items);
        }


        // Homework:
        // Add Post to add a new book
        [HttpPost("items/books")]
        public IActionResult AddBook([FromBody] Book book)
        {

            if (string.IsNullOrWhiteSpace(book.Title) || string.IsNullOrWhiteSpace(book.Author) || book.Pages < 0)
                return BadRequest("Invalid book data.");

            var addedBook = _service.AddBook(book.Title, book.Author, book.Pages);
            return CreatedAtAction(nameof(GetItems), new { id = addedBook.Id }, addedBook);
        }
    }
}
