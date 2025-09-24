using LibraryApp.Console.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Api;

public class LibraryController : ControllerBase
{
    private readonly LibraryService _service = new();
    
    //Add get to list all libreary items
    [HttpGet("items")]
    public IActionResult GetItems()
    {
        _service.Seed();
        var items = _service.Items;
        return Ok(items);
    }
}