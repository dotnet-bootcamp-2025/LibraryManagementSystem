using LibraryApp.Domain;
using LibraryApp.Application.Abstraction;
using Microsoft.AspNetCore.Mvc;
using LibraryApp.WebAPI.DTOs;

namespace LibraryApp.WebAPI.Controllers
{
    public class LibraryController : ControllerBase
    {
        private readonly ILibraryService _service;

        public LibraryController(ILibraryService libraryService)
        {
            _service = libraryService;
            // _libraryService.Seed();
        }

        [HttpGet("items")]
        public IActionResult GetItems()
        {
            var items = _service.GetAllLibraryItems();
            Console.WriteLine($"GET - ServiceCollection Instance : {_service.GetHashCode()}, Items Count: {items.Count()}");
            return Ok(items);
        }

        [HttpGet("listMembers")]
        public IActionResult ListMembers()
        {
            var members = _service.GetAllMembers();
            return Ok(members);
        }

        [HttpPost("book")]
        public IActionResult AddBook([FromBody] BookDto book)
        {
            if (book == null || string.IsNullOrWhiteSpace(book.Title) || string.IsNullOrWhiteSpace(book.Author))
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

        [HttpPost("magazine")]
        public IActionResult AddMagazine([FromBody] MagazineDto magazine)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var addedMagazine = _service.AddMagazine(magazine.Title, magazine.IssueNumber, magazine.Publisher);
            return CreatedAtAction(nameof(GetItems), new { id = addedMagazine.Id }, addedMagazine);
        }

        [HttpPost("registerMember")]
        public IActionResult RegisterMember([FromBody] RegisterMemberDto memberDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var registeredMember = _service.RegisterMember(memberDto.Name);
            return CreatedAtAction(nameof(GetItems), new { id = registeredMember.Id }, registeredMember);
        }

        [HttpPost("borrowItem")]
        public IActionResult BorrowItem([FromBody] BorrowDto borrowDetails)
        {
            if (borrowDetails == null) return BadRequest("Missing data.");
            var ok = _service.BorrowItem(borrowDetails.MemberId, borrowDetails.ItemId, out var msg);
            Console.WriteLine($"POST - Borrow Item successfully. MemberId: {borrowDetails.MemberId}, ItemId: {borrowDetails.ItemId}");
            if (ok) return Ok(new { success = ok, message = msg });
            return BadRequest(new { success = ok, message = msg });
        }

        [HttpPut("returnItem")]
        public IActionResult ReturnItem([FromBody] ReturnDto returnDetails)
        {
            if (returnDetails == null) return BadRequest("Missing data.");

            var ok = _service.ReturnItem(returnDetails.MemberId, returnDetails.ItemId);

            if (ok) return Ok(new { success = ok, message = "Item returned successfully." });

            return BadRequest(new { success = ok, message = "Failed to return item. Check member ID and item ID." });
        }
    }
}