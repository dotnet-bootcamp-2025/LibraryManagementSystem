using Microsoft.AspNetCore.Mvc;
using LibraryApp.Console.Services;

namespace LibraryApp.Api.Controllers
{
    //Add DTO class for creating a book
    public class BookCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int Pages { get; set; }
    }
    public class LibraryController : ControllerBase
    {
        //private static readonly LibraryService _service = new();
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
        public IActionResult AddBook([FromBody] BookCreateDto dto)
        {

            if (string.IsNullOrWhiteSpace(dto.Title) || string.IsNullOrWhiteSpace(dto.Author) || dto.Pages < 0)
                return BadRequest("Invalid book data.");

            var book = _service.AddBook(dto.Title, dto.Author, dto.Pages);
            return CreatedAtAction(nameof(GetItems), new { id = book.Id }, book);
        }
    }
}
