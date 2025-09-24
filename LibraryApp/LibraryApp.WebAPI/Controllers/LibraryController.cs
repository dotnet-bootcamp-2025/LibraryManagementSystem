using LibraryApp.Console.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.WebAPI.Controllers
{
    public class LibraryController : ControllerBase
    {
        private readonly LibraryService _libraryService = new();

        [HttpGet("items")]
        public IActionResult GetItems()
        {
            _libraryService.Seed();
            var items = _libraryService;
            return Ok(items);
        }
    }
}
