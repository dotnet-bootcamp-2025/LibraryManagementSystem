using LibraryApp.Api.Records;
using LibraryApp.Domain;
using LibraryApp.Services;
using Microsoft.AspNetCore.Mvc; 
using static LibraryApp.Api.Records.LibraryController;
using static LibraryApp.Api.Records.MagazineRecord;
using static LibraryApp.Api.Records.MemberRecord;

namespace LibraryApp.Api.Controllers
{
    public partial class LibraryController : ControllerBase
    {
        private readonly ILibraryService _service;

        public LibraryController(ILibraryService libraryservice)
        {
            _service = libraryservice;
            _service.Seed(); // Seed data for demo purposes
        }

        [HttpGet("items")]
        public IActionResult GetItems()
        {
            var items = _service.Items;
            return Ok(items);
        }

        //add POST to add a new book
        [HttpPost("Books")]
        public IActionResult AddBook([FromBody] CreateBookRequest Book)
        {
            if (Book == null || string.IsNullOrWhiteSpace(Book.Title) || string.IsNullOrWhiteSpace(Book.Author))
            {
                return BadRequest("Invalid book data.");
            }

            var addedBook=_service.AddBook(Book.Title, Book.Author, Book.Pages);
            return CreatedAtAction(nameof(GetItems), new { id = addedBook.Id }, addedBook);
        }

        [HttpPost("Magazine")]
        public IActionResult AddMagazine([FromBody] CreateMagazineRequest Magazine)
        {
            if (Magazine == null || string.IsNullOrWhiteSpace(Magazine.Title) || string.IsNullOrWhiteSpace(Magazine.Publisher))
            {
                return BadRequest("Invalid Magazine data.");
            }

            var addedMagazine = _service.AddMagazine(Magazine.Title, Magazine.IssueNumber, Magazine.Publisher);
            return CreatedAtAction(nameof(GetItems), new { id = addedMagazine.Id }, addedMagazine);
        }

        [HttpPost("Member")]
        public IActionResult RegisterMember([FromBody] CreateMemberRequest Member)
        {
            if (Member == null || string.IsNullOrWhiteSpace(Member.Name) || string.IsNullOrWhiteSpace(Member.Name))
            {
                return BadRequest("Invalid Magazine data.");
            }

            var member = _service.RegisterMember(Member.Name);
            return CreatedAtAction(nameof(GetItems), new { name = Member.Name });
        }

       [HttpPost("Borrow")]

       public IActionResult BorrowItem(int memberId, int itemId) { 
            if (memberId <= 0 || itemId <= 0)
            {
                return BadRequest("Invalid member or item ID.");
            }
            if (_service.BorrowItem(memberId, itemId, out string message))
            {
                return Ok("Item borrowed successfully.");
            }
            else
            {
                return BadRequest(message);
            }
        }

        [HttpPost("Return")]
        public IActionResult ReturnItem(int memberId, int itemId)
        {
            if (memberId <= 0 || itemId <= 0)
            {
                return BadRequest("Invalid member or item ID.");
            }
            if (_service.ReturnItem(memberId, itemId, out string message))
            {
                return Ok("Item returned successfully.");
            }
            else
            {
                return BadRequest(message);
            }
        }

        //TODO: add POST to add a new magazine, members, borrowing, and returning items


    }
}
