using LibraryApp.Api.DTOs;
using LibraryApp.Domain;
using LibraryApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Api.Controllers
{
    public class LibraryController : ControllerBase
    {
        // OLD approach -> private static readonly LibraryService _service = new LibraryService();
        private readonly ILibraryService _service;
        public LibraryController(ILibraryService libraryService)
        {
            _service = libraryService;
            _service.Seed();
        }

        #region GETs
        // Add GET to list all library items
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
        #endregion GETs

        #region POSTs
        [HttpPost("books")]
        public IActionResult AddBook([FromBody] CreateBookRequest book)
        {
            if (book == null || string.IsNullOrWhiteSpace(book.Title) || string.IsNullOrWhiteSpace(book.Author))
            {
                return BadRequest("Invalid book data.");
            }
            var addedBook = _service.AddBook(book.Title, book.Author, book.Pages);
            return CreatedAtAction(nameof(GetItems), new { id = addedBook.Id }, addedBook);
        }

        [HttpPost("magazines")]
        public IActionResult AddMagazine([FromBody] CreateMagazineRequest magazine)
        {
            if (magazine == null || string.IsNullOrWhiteSpace(magazine.Title) || magazine.IssueNumber <= 0 || string.IsNullOrWhiteSpace(magazine.Publisher))
            {
                return BadRequest("Invalid magazine data.");
            }
            var addedMagazine = _service.AddMagazine(magazine.Title, magazine.IssueNumber, magazine.Publisher);
            return CreatedAtAction(nameof(GetItems), new { id = addedMagazine.Id }, addedMagazine);
        }

        [HttpPost("members")]
        public IActionResult RegisterMember([FromBody] CreateMemberRequest member)
        {
            if (member == null || string.IsNullOrWhiteSpace(member.Name))
            {
                return BadRequest("Invalid member data.");
            }
            var addedMember = _service.RegisterMember(member.Name);
            return CreatedAtAction(nameof(GetMembers), new { id = addedMember.Id }, addedMember);
        }
        #endregion POSTs

        #region PATCHs
        [HttpPatch("borrowing")]
        public IActionResult BorrowItem([FromBody] CreateBorrowRequest borrow)
        {
            if (borrow == null || borrow.memberId <= 0 || borrow.itemId <= 0)
            {
                return BadRequest("Invalid member data.");
            }
            var addedBorrow = _service.BorrowItem(borrow.memberId, borrow.itemId, out var message);
            return Ok(message);
        }

        [HttpPatch("return")]
        public IActionResult ReturnItem([FromBody] CreateBorrowRequest borrow)
        {
            if (borrow == null || borrow.memberId <= 0 || borrow.itemId <= 0)
            {
                return BadRequest("Invalid member data.");
            }
            var addedBorrow = _service.ReturnItem(borrow.memberId, borrow.itemId, out var message);
            return Ok(message);
        }
        #endregion PATCHs
        // TODO: Add more endpoints for magazines, members, borrowing, and returning items
    }
}

// CQRS - Command Query Responsibility Segregation