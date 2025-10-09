using LibraryApp.Api.DTO;
using LibraryApp.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;
namespace ApiLibrary.Controllers;

public class LibraryController : ControllerBase
{
    private readonly ILibraryService _service;

    public LibraryController(ILibraryService libraryService)
    {
        _service = libraryService;
        Console.WriteLine($"Controller created with service instance: {_service.GetHashCode()}");
    }

    // GET: api/library/items
    [HttpGet("items")]
    public IActionResult GetItems()
    {
        Console.WriteLine($"GetItems called on controller with service instance: {_service.GetHashCode()}");
        var items = _service.GetAllLibraryItems();
        return Ok(items);
    }

    // POST: api/library/books
    [HttpPost("book")]
    public IActionResult AddBook([FromBody] BookDTO book)
    {
        if (book == null || string.IsNullOrEmpty(book.Title) || string.IsNullOrEmpty(book.Author))
        {
            return BadRequest("Invalid book data.");
        }

        var items = _service.GetAllLibraryItems();

        Console.WriteLine($"POST - Service instance: {_service.GetHashCode()}, Items count before: {items.Count()}");

        var addedBook = _service.AddBook(book.Title, book.Author, book.Pages);

        items = _service.GetAllLibraryItems();

        Console.WriteLine($"POST - Items count after: {items.Count()}");

        return CreatedAtAction(nameof(GetItems), new { id = addedBook.Id }, addedBook);
    }

    //// POST: api/library/magazines
    [HttpPost("magazine")]
    public IActionResult addMagazine([FromBody] MagazineDTO mag)
    {
        if (mag == null || string.IsNullOrEmpty(mag.Title) || string.IsNullOrEmpty(mag.Publisher))
        {
            return BadRequest("Invalid magazine data.");
        }

        var items = _service.GetAllLibraryItems();

        Console.WriteLine($"POST - Service instance: {_service.GetHashCode()}, Items count before: {items.Count()}");

        var addedMag = _service.AddMagazine(mag.Title, mag.IssueNumber, mag.Publisher);

        items = _service.GetAllLibraryItems();

        Console.WriteLine($"POST - Items count after: {items.Count()}");

        return CreatedAtAction(nameof(GetItems), new { id = addedMag.Id }, addedMag);
    }

    //// POST: api/library/members
    [HttpPost("member")]
    public IActionResult registerMember([FromBody] MemberDTO member)
    {
        if (member == null || string.IsNullOrEmpty(member.Name))
        {
            return BadRequest("Invalid member data.");
        }
        var addedMember = _service.RegisterMember(member.Name);
        return CreatedAtAction(nameof(GetItems), new { id = addedMember.Id }, addedMember);
    }

    //// GET: api/library/members
    [HttpGet("members")]
    public IActionResult GetMembers()
    {
        var members = _service.GetAllMembers();
        Console.WriteLine($"GET - Service instance: {_service.GetHashCode()}, Members count: {members.Count()}");
        return Ok(members);
    }


    //// POST: api/library/borrow
    [HttpPost("borrow")]
    public IActionResult borrowItem([FromBody] BorrowReturnDTO itemId)
    {
        if (itemId == null || itemId.MemberId <= 0 || itemId.ItemId <= 0)
        {
            return BadRequest("Invalid borrow data.");
        }
        var success = _service.BorrowItem(itemId.MemberId, itemId.ItemId, out string message);
        if (success)
        {
            return Ok(message);
        }
        else
        {
            return BadRequest(message);
        }
    }

    //// POST: api/library/return
    [HttpPost("return")]
    public IActionResult returnItem([FromBody] BorrowReturnDTO itemId)
    {
        if (itemId == null || itemId.MemberId <= 0 || itemId.ItemId <= 0)
        {
            return BadRequest("Invalid borrow data.");
        }
        var success = _service.ReturnItem(itemId.MemberId, itemId.ItemId, out string message);
        if (success)
        {
            return Ok(message);
        }
        else
        {
            return BadRequest(message);
        }
    }
}