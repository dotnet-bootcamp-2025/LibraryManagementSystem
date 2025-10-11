using LibraryApp.Application.Abstraction;
using LibraryApp.WebAPI.DTOs;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("findItems")]
        public IActionResult FindItems([FromQuery] string? term)
        {
            var items = _service.FindItems(term);

            if (!items.Any())
                return NotFound(new { message = $"No items were found that match '{term}'." });

            return Ok(items);
        }

        [HttpGet("listMembers")]
        public IActionResult ListMembers()
        {
            var members = _service.GetAllMembers();
            return Ok(members);
        }

        [HttpGet("memberById")]
        public IActionResult GetMemberLoans(int memberId)
        {
            if (!_service.MemberExists(memberId))
            {
                return NotFound(new { success = false, message = "Member not found." });
            }

            var loans = _service.GetMemberActiveLoans(memberId);
            return Ok(loans);
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
        public IActionResult BorrowItem([FromBody] BorrowDto request)
        {
            var success = _service.BorrowItem(request.MemberId, request.ItemId, out string message, out string? formattedReturnDate);

            if (!success)
            {
                return BadRequest(new { success = false, message = message });
            }

            return Ok(new
            {
                success = true,
                message = message,
                data = new
                {
                    returnDate = formattedReturnDate
                }
            });
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