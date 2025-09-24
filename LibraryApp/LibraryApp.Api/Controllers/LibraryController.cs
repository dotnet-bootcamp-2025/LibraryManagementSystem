using LibraryApp.Console.Services;
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
        [HttpPost("book")]
        public IActionResult PostBook(string title, string author, int pages = 0)
        {
            var book = _service.AddBook(title, author, pages);
            return Ok(book);
        }
    }
}
