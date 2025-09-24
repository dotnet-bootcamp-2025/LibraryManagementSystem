using LibraryApp.Console.Domain;
using LibraryApp.Console.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Api;

public class LibraryController : ControllerBase
{
    private readonly LibraryService _service = new();
    //al hacer unit testing ayuda mucho esto, ya que Library controller crea una interface que hace dependencia de arriba hacia abajo no de abajo hacia arriba
    
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
    public IActionResult AddBook([FromBody] BookReq book)
    {
        var newBook = _service.AddBook(book.Title, book.Author, book.Pages);
        return Created("books/" + newBook.Id, newBook);
    }
}

public record BookReq(string Title, string Author, int Pages);