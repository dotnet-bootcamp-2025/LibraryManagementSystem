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

    [HttpGet("items")]
    public IActionResult GetItems()
    {
        var items = _service.Items;

        return Ok(items);
    }

    [HttpGet("members")]
    public IActionResult GetMembers()
    {
        var members = _service.Members;

        return Ok(members);
    }

    [HttpPost("addBook")]
    public IActionResult AddBook([FromBody] BookDTO book)
    {
        if (book == null || string.IsNullOrWhiteSpace(book.Title) || string.IsNullOrWhiteSpace(book.Author))
        {
            return BadRequest("Invalid book data");
        }
        var addBook = _service.AddBook(book.Title, book.Author, book.Pages);

        return CreatedAtAction(nameof(GetItems), new { id = addBook.Id, addBook });
    }

    [HttpPost("addMagazzine")]
    public IActionResult AddMagazzine([FromBody] MagazineDTO magazzine)
    {
        if (magazzine == null || string.IsNullOrWhiteSpace(magazzine.Title) || string.IsNullOrWhiteSpace(magazzine.Publisher) || magazzine.IssueNumber <= 0)
        {
            return BadRequest("Invalid magazzine data");
        }

        var addMagazzine = _service.AddMagazine(magazzine.Title, magazzine.IssueNumber, magazzine.Publisher);

        return CreatedAtAction(nameof(GetItems), new { id = addMagazzine.Id, addMagazzine });
    }

    [HttpPost("registerMember")]
    public IActionResult AddMember([FromBody] MemberDTO member)
    {
        if (member == null || string.IsNullOrWhiteSpace(member.Name))
        {
            return BadRequest("Invalid member data");
        }

        var addMember = _service.RegisterMember(member.Name);

        return CreatedAtAction(nameof(GetMembers), new { id = addMember.Id, addMember });
    }

    [HttpPost("borrowItem")]
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

    [HttpPost("returnItem")]
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