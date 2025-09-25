using LibraryApp.Domain;
using LibraryApp.Services;
using Microsoft.AspNetCore.Mvc; 

namespace LibraryApp.Api.Controllers
{
    public class LibraryController : ControllerBase
    {
        private readonly ILibraryService _service;

        public LibraryController(ILibraryService libraryservice)
        {
            _service = libraryservice;
        }

        [HttpGet("items")]
        public IActionResult GetItems()
        {
            _service.Seed(); // Seed data for demo purposes
            var items = _service.Items;
            return Ok(items);
        }

        //add POST to add a new book
        [HttpPost("Books")]
        public IActionResult AddBook([FromBody] Book Book)
        {
            if (Book == null || string.IsNullOrWhiteSpace(Book.Title) || string.IsNullOrWhiteSpace(Book.Author))
            {
                return BadRequest("Invalid book data.");
            }

            var addedBook=_service.AddBook(Book.Title, Book.Author, Book.Pages);
            return CreatedAtAction(nameof(GetItems), new { id = addedBook.Id }, addedBook);
        }
    }
}
