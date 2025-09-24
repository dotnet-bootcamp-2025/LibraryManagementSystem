using LibraryApp.Console.Domain;
using LibraryApp.Console.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Api.Controllers
{
    public class LibraryController : ControllerBase
    {
        //private readonly LibraryService _service = new();
        private readonly LibraryService _service;
        
        public LibraryController(LibraryService libraryService)
        {
            _service = libraryService;
        }

        // Add GET to list all library items
        [HttpGet("items")]
        public IActionResult GetItems()
        {
            _service.Seed(); // Seed data for demonstration
            var items = _service.Items;
            return Ok(items);
        }

        // Homework: Add POST to add a new book

        [HttpPost("books")]

        public IActionResult AddBook([FromBody] Book bookDto)
        {
            if (bookDto == null || string.IsNullOrWhiteSpace(bookDto.Title) || string.IsNullOrWhiteSpace(bookDto.Author))
            {
                return BadRequest("Invalid book data.");
            }
            var book = _service.AddBook(bookDto.Title, bookDto.Author, bookDto.Pages);
            return CreatedAtAction(nameof(GetItems), new { id = book.Id }, book);
        }

    }
}
