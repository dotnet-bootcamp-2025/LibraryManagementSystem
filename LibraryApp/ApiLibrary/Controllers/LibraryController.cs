using LibraryApp.Services;
using LibraryApp.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ApiLibrary.Controllers
{
    public class LibraryController : ControllerBase
    {
        private readonly ILibraryService _service;

        private ILogger logger;

        public LibraryController(ILibraryService libraryService, ILogger logger)
        {
            _service = libraryService;
            this.logger = logger;
        }

        [HttpGet("items")]
        public IActionResult GetItems()
        {
            _service.Seed();
            var items = _service.Items;

            return Ok(items);
        }

        // add post ot adda a new book
        // tarea hacer que el library app jale con las 

        [HttpPost("item")]
        public IActionResult AddBook([FromBody] Book book)
        {
            if(book == null || string.IsNullOrWhiteSpace(book.Title) || string.IsNullOrWhiteSpace(book.Author))
            {
                return BadRequest("Invalid book data");
            }
            var addBook = _service.AddBook(book.Title, book.Author, book.Pages);

            return CreatedAtAction(nameof(GetItems), new {  id = addBook.Id, addBook});
        }
    }
}
