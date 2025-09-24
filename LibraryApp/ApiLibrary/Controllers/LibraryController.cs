using LibraryApp.Console.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiLibrary.Controllers
{
    public class LibraryController : ControllerBase
    {
        private readonly ILibraryService _service;

        private ILogger logger;

        public LibraryController(ILibraryService libraryService, ILogger logger)
        {
            _service = libraryService;
            this.logger = logger;
        }

        [HttpGet("items")]
        public IActionResult GetItems()
        {
            _service.Seed();
            var items = _service.Items;

            return Ok(items);
        }
    }
}
