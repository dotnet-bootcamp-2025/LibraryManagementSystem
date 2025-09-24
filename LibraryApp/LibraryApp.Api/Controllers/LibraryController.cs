using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LibraryApp.Console.Services;
using LibraryApp.Console.Domain;

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
        public IActionResult GetSeedData()
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
    }
}
