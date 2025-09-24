using LibraryApp.Console.Services;
using Microsoft.AspNetCore.Mvc; 

namespace LibraryApp.Api.Controllers
{
    public class LibraryController : ControllerBase
    {
        private readonly ILibraryService _service;

        public LibraryController(ILibraryService libraryservice)
        {
            _service = libraryservice;
        }

        [HttpGet("items")]
        public IActionResult GetItems()
        {
            _service.Seed(); // Seed data for demo purposes
            var items = _service.Items;
            return Ok(items);
        }

        //add POST to add a new book

    }
}
