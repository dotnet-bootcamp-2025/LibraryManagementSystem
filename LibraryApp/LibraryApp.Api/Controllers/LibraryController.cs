using Microsoft.AspNetCore.Mvc;
using LibraryApp.Domain;
using LibraryApp.Services;

namespace LibraryApp.Api.Controllers
{
    public class LibraryController : ControllerBase
    {
        //OLD approach -> private readonly LibraryService _service = new();
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

        // TODO : Add more endpoints for magazines, members, borrowing and returning items

    }
}
