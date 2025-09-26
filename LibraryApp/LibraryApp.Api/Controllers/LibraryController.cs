using Microsoft.AspNetCore.Mvc;
using LibraryApp.Domain;
using LibraryApp.Services;
using LibraryApp.Api.DTOs;

namespace LibraryApp.Api.Controllers
{
    public class LibraryController : ControllerBase
    {
        //OLD private static readonly LibraryService _service = new();
        private readonly ILibraryService _service;
        private static bool _isSeeded = false;
        private static readonly object _seedLock = new object();

        public LibraryController(ILibraryService LibraryService )
        {
            _service = LibraryService;
            if (!_isSeeded)
            {
                lock (_seedLock)
                    {
                    if (!_isSeeded)
                    {
                        _service.Seed();
                        _isSeeded = true;
                    }
                }
            }
        }

        //Add GET to list all library items
        [HttpGet("items")]
        public IActionResult GetItems()
        {
            var items = _service.Items;
            return Ok(items);
        }


        // Homework:
        // Add Post to add a new book
        [HttpPost("books")]
        
        public IActionResult AddBook([FromBody] AddBookDTO book)
        {
            if (book == null || string.IsNullOrWhiteSpace(book.Title) || string.IsNullOrWhiteSpace(book.Author))
            {
                return BadRequest("Invalid book data.");
            }
            var newBook = _service.AddBook(book.Title, book.Author, book.Pages);
            return CreatedAtAction(nameof(GetItems), new { id = newBook.Id }, newBook);
        }

        //TODO : Add more endpoints for magazines, members, borrowing, and returning items.

        [HttpPost("magazines")]
        public IActionResult AddMagazine([FromBody] AddMagazineDTO magazine)
        {
            if (magazine == null || string.IsNullOrWhiteSpace(magazine.Title) || string.IsNullOrWhiteSpace(magazine.Publisher))
            {
                return BadRequest("Invalid magazine data.");
            }
            var newMagazine = _service.AddMagazine(magazine.Title, magazine.IssueNumber, magazine.Publisher);
            return CreatedAtAction(nameof(GetItems), new { id = newMagazine.Id }, newMagazine);
        }

        [HttpGet("members")]
        public IActionResult GetMembers()
        {
            var members = _service.Members;
            return Ok(members);
        }

        [HttpPost("members")]
        public IActionResult RegisterMember([FromBody] RegisterMemberDTO member)
        {
            if (member == null || string.IsNullOrWhiteSpace(member.Name))
            {
                return BadRequest("Invalid member data.");
            }
            var newMember = _service.RegisterMember(member.Name);
            return CreatedAtAction(nameof(GetMembers), new { id = newMember.Id }, newMember);
        }

        [HttpPost("borrow")]
        public IActionResult BorrowItem([FromBody] BorrowDTO borrowRequest)
        {
            if (borrowRequest == null || borrowRequest.MemberId <= 0 || borrowRequest.ItemId <= 0)
            {
                return BadRequest("Invalid borrow request data.");
            }
            if (_service.BorrowItem(borrowRequest.MemberId, borrowRequest.ItemId, out string message))
            {
                return Ok(message);
            }
            return BadRequest(message);
        }

        [HttpPost("return")]
        public IActionResult ReturnItem([FromBody] ReturnDTO returnRequest)
        {
            if (returnRequest == null || returnRequest.MemberId <= 0 || returnRequest.ItemId <= 0)
            {
                return BadRequest("Invalid return request data.");
            }
            if (_service.ReturnItem(returnRequest.MemberId, returnRequest.ItemId, out string message))
            {
                return Ok(message);
            }
            return BadRequest(message);
        }


    }
}
