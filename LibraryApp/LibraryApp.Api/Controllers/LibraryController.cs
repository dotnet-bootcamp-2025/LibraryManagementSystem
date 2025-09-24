using Microsoft.AspNetCore.Mvc;
using LibraryApp.Console.Services;
using LibraryApp.Console.Domain;
using System.Security.Cryptography.X509Certificates;

namespace LibraryApp.Api.Controllers
{
    public class LibraryController : ControllerBase
    {
        private readonly LibraryService _service = new();
        public LibraryController(ILibraryService libraryService)
        {
            _service = (LibraryService)libraryService;
        }
        [HttpGet("items")]
        public IActionResult GetItems()
        {
            _service.Seed();
            var items = _service;
            return Ok(items);
        }
            //Add POST to add a new book

            [HttpPost("book")]
        public IActionResult AddBook([FromBody] Book book)
            {
                var addedBook = _service.AddBook(book.Title, book.Author, book.Pages);

                return Ok(addedBook);

            
        }
    }
}