using Microsoft.AspNetCore.Mvc;
using LibraryApp.Console.Services;

namespace LibraryApp.Api.Controllers
{
    public class LibraryController : ControllerBase
    {
        //private static readonly LibraryService _service = new();
        private readonly ILibraryService _service;

        public LibraryController(ILibraryService LibraryService )
        {
            _service = LibraryService;
        }

        //Add GET to list all library items
        [HttpGet("items")]
        public IActionResult GetItems()
        {
            _service.Seed();
            var items = _service.Items;
            return Ok(items);
        }

    }
}
