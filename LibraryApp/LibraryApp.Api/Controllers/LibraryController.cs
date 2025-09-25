using LibraryApp.Domain;
using LibraryApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Api.Controllers
{
    public class LibraryController : ControllerBase
    {
        //private static readonly LibraryService _service = new(); // MALA PRÁCTICA, CREA DEPENDENCIAS
        private readonly ILibraryService _service;

        public LibraryController(ILibraryService libraryService)
        {
            _service = libraryService;
        }

        // Add GET to list all library items
        [HttpGet("items")]
        // My Solution
        //public IReadOnlyList<LibraryItem> Get()
        //{
        //    _service.Seed();
        //    return _service.Items.ToArray();
        //}
        // Mike's Solution
        public IActionResult GetItems()
        {
            _service.Seed();
            var items = _service.Items;
            return Ok(items);
        }

        // Add POST to add a new book
        //[HttpPost("book")]
        //// My Version
        //public IActionResult PostBook(string title, string author, int pages = 0)
        //{
        //    var book = _service.AddBook(title, author, pages);
        //    return Ok(book);
        //}

        // Mike's Version
        [HttpPost("books")]
        public IActionResult AddBook([FromBody] Book book)
        {
            if (book == null || string.IsNullOrEmpty(book.Title) || string.IsNullOrEmpty(book.Author))
            {
                return BadRequest("Invalid book data.");
            }
            var addedBook = _service.AddBook(book.Title, book.Author, book.Pages);
            return CreatedAtAction(nameof(GetItems), new { id = addedBook.Id }, addedBook);
        }
    }
}
