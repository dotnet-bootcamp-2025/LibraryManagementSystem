using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Api.Controllers
{
    public class LibraryController : ControllerBase
    {
        private readonly LibraryService _service = new();

        //add GET to list all library items
        [HttpGet("items")]  
        public IActionResult GetItems()
        {
            _service.Seed(); // Seed data for demo purposes
            var items = _service.Items;
            return Ok(items);
        }

    }
}
