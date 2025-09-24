using LibraryApp.Console.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Api.Controllers
{
    public class LibraryController : ControllerBase
    {
        //OLD approach -> private readonly LibraryService _service = new LibraryService();
        private readonly ILibraryService _service;

        public LibraryController(ILibraryService libraryService)
        {
            _service = libraryService;
        }

        // Add GET to list all library items
        [HttpGet("items")]
        public IActionResult GetItems()
        {
            _service.Seed(); // Seed data for demo purposes
            var items = _service.Items;
            return Ok(items);
        }
    }
}
