using LibraryApp.Api.DTO;
using LibraryApp.Services;
using Microsoft.AspNetCore.Mvc;
namespace ApiLibrary.Controllers;

public class LibraryController : ControllerBase
{
    private readonly ILibraryService _service;

    public LibraryController(ILibraryService libraryService)
    {
        _service = libraryService;
        _service.Seed();
    }

    // GET: api/library/items
    [HttpGet("items")]
    public IActionResult GetItems()
    {
        var items = _service.Items;
        return Ok(items);
    }

    // POST: api/library/books
    [HttpPost("books")]
    public IActionResult AddBook([FromBody] BookDTO book)
    {
        if (book == null || string.IsNullOrWhiteSpace(book.Title) || string.IsNullOrWhiteSpace(book.Author))
            return BadRequest("Invalid book data");

        var addBook = _service.AddBook(book.Title, book.Author, book.Pages);
        return CreatedAtAction(nameof(GetItems), new { id = addBook.Id }, addBook);
    }

    // POST: api/library/magazines
    [HttpPost("magazines")]
    public IActionResult AddMagazine([FromBody] MagazineDTO magazine)
    {
        if (magazine == null || string.IsNullOrWhiteSpace(magazine.Title) ||
            magazine.IssueNumber <= 0 || string.IsNullOrWhiteSpace(magazine.Publisher))
        {
            return BadRequest("Invalid magazine data");
        }

        var addedMagazine = _service.AddMagazine(magazine.Title, magazine.IssueNumber, magazine.Publisher);
        return CreatedAtAction(nameof(GetItems), new { id = addedMagazine.Id }, addedMagazine);
    }

    // POST: api/library/members
    [HttpPost("members")]
    public IActionResult RegisterMember([FromBody] MemberDTO member)
    {
        if (member == null || string.IsNullOrWhiteSpace(member.Name))
            return BadRequest("Invalid member data");

        var registeredMember = _service.RegisterMember(member.Name);
        return CreatedAtAction(nameof(GetMembers), new { id = registeredMember.Id }, registeredMember);
    }

    // GET: api/library/members
    [HttpGet("members")]
    public IActionResult GetMembers()
    {
        var members = _service.Members;
        return Ok(members);
    }

    // POST: api/library/borrow
    [HttpPost("borrow")]
    public IActionResult BorrowItem(int memberId, int itemId)
    {
        if (memberId <= 0 || itemId <= 0)
        {
            return BadRequest("Invalid member item / library item identification");
        }

        string resultMessage;

        var result = _service.BorrowItem(memberId, itemId, out resultMessage);

        return CreatedAtAction(nameof(GetItems), nameof(GetMembers), resultMessage);
    }

    // POST: api/library/return
    [HttpPost("return")]
    public IActionResult ReturnItem(int memberId, int itemId)
    {
        if (memberId <= 0 || itemId <= 0)
        {
            return BadRequest("Invalid member item / library item identification");
        }

        string resultMessage;

        var result = _service.ReturnItem(memberId, itemId, out resultMessage);

        return CreatedAtAction(nameof(GetItems), nameof(GetMembers), resultMessage);
    }
}