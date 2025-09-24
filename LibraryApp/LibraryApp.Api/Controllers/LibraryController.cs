using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LibraryApp.Console.Services;

namespace LibraryApp.Api.Controllers
{
    public class LibraryController : ControllerBase
    {
        private readonly LibraryService _service = new();

        [HttpGet("items")]
        public IActionResult GetSeedData()
        {
            _service.Seed();
            var items = _service.Items;
            return Ok(items);

        }
    }
}
