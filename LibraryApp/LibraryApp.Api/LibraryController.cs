
using LibraryApp.Domain;
using LibraryApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Api;

public class LibraryController : ControllerBase
{
    // old private readonly LibraryService _service = new();
    private readonly ILibraryService _service;
    
    //al hacer unit testing ayuda mucho esto, ya que Library controller crea una interface que hace dependencia de arriba hacia abajo no de abajo hacia arriba
    public LibraryController(ILibraryService libraryService)
    {
        _service = libraryService;
    }
    //en clase el extra es una instancia donde se inyecta que controllador tomamos de LibraryService. es más recomendada en forma 2. Deja menos code deb.
    //Add get to list all library items
    
    
    //Add Post a new book
    [HttpGet("items")]
    public IActionResult GetItems()
    {
        _service.Seed();
        var items = _service.Items;
        return Ok(items);
    }

    [HttpPost("books")]
    public IActionResult AddBook([FromBody] Book book)
    {
        if (book == null || string.IsNullOrWhiteSpace(book.Title) || string.IsNullOrWhiteSpace(book.Author))
        {
            return BadRequest("Invalid book data.");
        }

        var addedBook = _service.AddBook(book.Title, book.Author, book.Pages);
        return CreatedAtAction(nameof(GetItems), new {id=addedBook.Id},addedBook);
    }
}