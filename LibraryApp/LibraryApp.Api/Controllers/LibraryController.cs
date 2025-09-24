using LibraryApp.Console.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Api.Controllers
{
    public class LibraryController : ControllerBase
    {
        //private readonly LibraryService _service = new();
        private readonly LibraryService _service;
        
        public LibraryController(LibraryService libraryService)
        {
            _service = libraryService;
        }

        // Add GET to list all library items
        [HttpGet("items")]
        public IActionResult GetItems()
        {
            _service.Seed(); // Seed data for demonstration
            var items = _service.Items;
            return Ok(items);
        }

    }
}
