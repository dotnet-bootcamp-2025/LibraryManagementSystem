using Microsoft.AspNetCore.Mvc;
using LibraryApp.Console.Services;

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



            /*Add POST to add a new book

            [HttpPost("book")]
            public IActionResult AddBook([FromBody] Book book)
            {
                if (book == null)
                    return BadRequest("Book data is required.");

                _service.AddBook(book);
                return CreatedAtAction(nameof(GetItems), new { id = book.Id }, book);*/

        }
    }
}