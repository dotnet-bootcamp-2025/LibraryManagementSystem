using Microsoft.AspNetCore.Mvc;
using LibraryApp.ConsoleApp;
using LibraryApp.ConsoleApp.Domain;

namespace LibraryApp.Api.Controllers
{
    public class LibraryController : ControllerBase
    {
        // OLD approach -> private static readonly LibraryService _service = new LibraryService();
        private readonly ILibraryService _service;
        public LibraryController(ILibraryService libraryService)
        {
            _service = libraryService;
        }
        // Add GET to list all library items
        [HttpGet("items")]
        public IActionResult GetItems()
        {
            _service.Seed();
            var items = _service.Items;
            return Ok(items);
        }
    }
}