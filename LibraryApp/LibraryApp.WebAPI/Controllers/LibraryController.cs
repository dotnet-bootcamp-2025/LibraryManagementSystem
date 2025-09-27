using LibraryApp.Domain;
using LibraryApp.Services;
using Microsoft.AspNetCore.Mvc;
using LibraryApp.WebAPI.DTOs;

namespace LibraryApp.WebAPI.Controllers
{
    public class LibraryController : ControllerBase
    {
        private readonly ILibraryService _libraryService;

        public LibraryController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        [HttpGet("items")]
        public IActionResult GetItems()
        {
            _libraryService.Seed(); // Seed data for demonstration purposes
            var items = _libraryService.Items;
            return Ok(items);
        }

        [HttpGet("listMembers")]
        public IActionResult ListMembers()
        {
            _libraryService.Seed();
            var members = _libraryService.Members;
            return Ok(members);
        }

        [HttpPost("book")]
        public IActionResult AddBook([FromBody] BookDto book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var addedBook = _libraryService.AddBook(book.Title, book.Author, book.Pages);
            return CreatedAtAction(nameof(GetItems), new { id = addedBook.Id }, addedBook);
        }

        [HttpPost("magazine")]
        public IActionResult AddMagazine([FromBody] MagazineDto magazine)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var addedMagazine = _libraryService.AddMagazine(magazine.Title, magazine.IssueNumber, magazine.Publisher);
            return CreatedAtAction(nameof(GetItems), new { id = addedMagazine.Id }, addedMagazine);
        }

        [HttpPost("registerMember")]
        public IActionResult RegisterMember([FromBody] RegisterMemberDto memberDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var registeredMember = _libraryService.RegisterMember(memberDto.Name);
            return CreatedAtAction(nameof(GetItems), new { id = registeredMember.Id }, registeredMember);
        }

        [HttpPost("borrowItem")]
        public IActionResult BorrowItem([FromBody] BorrowDto borrowDetails)
        {
            try
            {
                _libraryService.BorrowItem(borrowDetails.MemberId, borrowDetails.ItemId);

                return Ok(new { message = "Item borrowed successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("returnItem")]
        public IActionResult ReturnItem([FromBody] ReturnDto returnDetails)
        {
            try
            {
                _libraryService.ReturnItem(returnDetails.MemberId, returnDetails.ItemId);
                return Ok(new { message = "Item returned successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}