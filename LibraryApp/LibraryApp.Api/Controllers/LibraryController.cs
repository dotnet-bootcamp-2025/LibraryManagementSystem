using Microsoft.AspNetCore.Mvc;
using LibraryApp.Domain;
using LibraryApp.Services;
using LibraryApp.Api.Dtos;

namespace LibraryApp.Api.Controllers
{
    public class LibraryController : ControllerBase
    {
        //OLD approach -> private readonly LibraryService _service = new();
        private readonly ILibraryService _service;
        
        public LibraryController(ILibraryService libraryService)
        {
            _service = libraryService;
            _service.Seed(); // Seed data for demonstration
        }

        // Add GET to list all library items
        [HttpGet("items")]
        public IActionResult GetItems()
        {
            var items = _service.Items;
            return Ok(items);
        }

        // Homework: Add POST to add a new book
        [HttpPost("books")]
        public IActionResult AddBook([FromBody] BookDTO bookDto)
        {
            if (bookDto == null || string.IsNullOrWhiteSpace(bookDto.Title) || string.IsNullOrWhiteSpace(bookDto.Author))
            {
                return BadRequest("Invalid book data.");
            }
            var book = _service.AddBook(bookDto.Title, bookDto.Author, bookDto.Pages);
            return CreatedAtAction(nameof(GetItems), new { id = book.Id }, book);
        }

        // TODO : Add more endpoints for magazines, members, borrowing and returning items

        // Add POST to add a new magazine
        [HttpPost("magazines")]
        public IActionResult AddMagazine([FromBody] MagazineDTO magDto)
        {
            if (magDto == null || string.IsNullOrWhiteSpace(magDto.Title) || string.IsNullOrWhiteSpace(magDto.Publisher) || magDto.IssueNumber <= 0)
            {
                return BadRequest("Invalid magazine data.");
            }
            var mag = _service.AddMagazine(magDto.Title, magDto.IssueNumber, magDto.Publisher);
            return CreatedAtAction(nameof(GetItems), new { id = mag.Id }, mag);
        }

        // Add POST to register a new member
        [HttpPost("members")]
        public IActionResult RegisterMember([FromBody] MemberDTO memberDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var member = _service.RegisterMember(memberDto.Name);
            return CreatedAtAction(nameof(GetItems), new { id = member.Id }, member);
        }

        // Add PATCH to borrow an item
        [HttpPatch("borrow")]
        public IActionResult BorrowItem(int memberId, int itemId)
        {
            if (_service.BorrowItem(memberId, itemId, out string message))
            {
                return Ok(message);
            }
            return BadRequest(message);
        }

        // Add PATCH to return an item
        [HttpPatch("return")]
        public IActionResult ReturnItem(int memberId, int itemId)
        {
            if (_service.ReturnItem(memberId, itemId, out string message))
            {
                return Ok(message);
            }
            return BadRequest(message);
        }

    }
}
