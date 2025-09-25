using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LibraryApp.Domain;
using LibraryApp.Services;

namespace LibraryApp.Api.Controllers
{
    public class LibraryController : ControllerBase
    {
        private readonly ILibraryService _service;

        public LibraryController(ILibraryService service)
        {
            _service = service;
        }

        [HttpGet("items")]
        public IActionResult GetItems()
        {
            _service.Seed();
            var items = _service.Items;
            return Ok(items);

        }

        [HttpPost("books")]
        public ActionResult<Book> RegisterBook(BookDto dto)
        {
            var book = _service.AddBook(dto.Title!, dto.Author!, dto.Pages);
            return Ok(book);
        }


        //IDEAL APPROACH

        //public IActionResult AddBook([FromBody] Book book)
        //{
        //    if (book == null || string.IsNullOrWhiteSpace(book.Title) || string.IsNullOrWhiteSpace(book.Author))
        //    {
        //        return BadRequest("Invalid book data");
        //    }

        //    var addedBook = _service.AddBook(book.Title, book.Author, book.Pages);
        //    return CreatedAtAction(nameof(GetItems), new { id = addedBook.Id }, addedBook);
        //}
    }
}
