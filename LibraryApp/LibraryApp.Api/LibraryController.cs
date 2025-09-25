
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

    [HttpPost("magazines")]
    public IActionResult AddMagazine([FromBody] Magazine magazine)
    {
        if (magazine == null || string.IsNullOrWhiteSpace(magazine.Title)|| int.IsNegative(magazine.IssueNumber) || string.IsNullOrEmpty(magazine.Publisher))
        {
            return BadRequest("Invalid magazine data.");
        }
        var addedMagazine = _service.AddMagazine(magazine.Title, magazine.IssueNumber, magazine.Publisher);
        return CreatedAtAction(nameof(GetItems), new {id=addedMagazine.Id},addedMagazine);
    }

    [HttpPost("members")]
    public IActionResult AddMember([FromBody] Member member)
    {
        if (member == null || string.IsNullOrWhiteSpace(member.Name))
        {
            return BadRequest("Invalid member data.");
        }
        var addedMember = _service.RegisterMember(member.Name);
        return CreatedAtAction(nameof(GetItems), new {id=addedMember.Id},addedMember);
    }

    [HttpGet("searchItems")]
    public IActionResult SearchItems([FromQuery] string term)
    {
        if (term == null || string.IsNullOrWhiteSpace(term))
        {
            return BadRequest("Invalid term data.");
        }

        var searchRes = _service.FindItems(term).ToList();
        return Ok(searchRes);
    }
    
}