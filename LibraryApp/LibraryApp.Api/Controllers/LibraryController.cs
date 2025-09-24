using LibraryApp.console.Domain;
using LibraryApp.console.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Api.Controllers
{
    public class LibraryController : ControllerBase
    {
       // private readonly LibraryService _service = new LibraryService();
        private readonly ILibraryService _service;
        
        public LibraryController(ILibraryService libraryService)
        {
            _service = libraryService;            
        }

        //add GET to list all library items
        [HttpGet("items")]  
        public IActionResult GetItems()
        {
            _service.Seed(); // Seed data for demo purposes
            var items = _service.Items;
            return Ok(items);
        }

        [HttpPost("add-book")]
        public IActionResult AddBook([FromBody] AddBookRequest request)
        {
            // 1. Validate the request data.
            if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Author))
            {
                // Return a 400 Bad Request if the required fields are missing.
                return BadRequest("Title and Author are required.");
            }

            // 2. Call the service to add the book.
            var newBook = _service.AddBook(request.Title, request.Author, request.Pages);

            // 3. Return a 201 Created status code with the newly created book.
            // The Url.Action method helps generate the URL for the created resource.
            return CreatedAtAction(nameof(GetItems), new { id = newBook.Id }, newBook);
        }


    }
}
