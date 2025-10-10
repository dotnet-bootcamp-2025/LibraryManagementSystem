using LibraryApp.Application.Abstractions;
using LibraryApp.Services.Records;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Api.Controllers
{
    public class LibraryController : ControllerBase
    {
        private readonly ILibraryService _service;

        public LibraryController(ILibraryService service)
        {
            _service = service;
        }

        #region GET

        [HttpGet("items")]
        public IActionResult GetItems([FromQuery] string term = "")
        {
            var items = _service.FindItems(term);
            return Ok(items);
        }

        [HttpGet("members")]
        public IActionResult GetMembers()
        {
            var items = _service.GetAllMembers();
            return Ok(items);
        }

        [HttpGet("members/{memberId}")]
        public IActionResult GetMemberById(int memberId)
        {
            if (memberId <= 0)
            {
                var error = "Invalid member ID data";
                return BadRequest(new { error });
            }

            var (success, member) = _service.GetMemberById(memberId, out var message);

            if (!success)
            {
                return NotFound(new { message });
            }

            return Ok(new { message, member });
        }

        [HttpGet("members/{memberId}/borrowed-items")]
        public IActionResult GetMemberBorrowedItems(int memberId)
        {
            if (memberId <= 0)
            {
                var error = "Invalid member ID data.";
                return BadRequest(new { error });
            }

            var (success, items) = _service.GetMemberBorrowedItems(memberId, out var message);

            if (!success)
            {
                return NotFound(new { message });
            }

            return Ok(new { message, items });
        }

        #endregion GET

        #region POST

        [HttpPost("books")]
        public IActionResult AddBook([FromBody] BookRecord bookRecord)
        {
            if (bookRecord == null || string.IsNullOrWhiteSpace(bookRecord.Title)
                || string.IsNullOrWhiteSpace(bookRecord.Author))
            {
                var error = "Invalid book data.";
                return BadRequest(new { error });
            }

            var addedBook = _service.AddBook(bookRecord.Title, bookRecord.Author, bookRecord.Pages);
            return CreatedAtAction(nameof(GetItems), new { id = addedBook.Id }, addedBook);
        }

        [HttpPost("magazines")]
        public IActionResult AddMagazine([FromBody] MagazineRecord magRecord)
        {
            if (magRecord == null || string.IsNullOrWhiteSpace(magRecord.Title)
                || int.IsNegative(magRecord.IssueNumber)
                || string.IsNullOrWhiteSpace(magRecord.Publisher))
            {
                var error = "Invalid magazine data.";
                return BadRequest(new { error });
            }

            var addedMag = _service.AddMagazine(magRecord.Title, magRecord.IssueNumber, magRecord.Publisher);
            return CreatedAtAction(nameof(GetItems), new { id = addedMag.Id }, addedMag);
        }

        [HttpPost("members")]
        public IActionResult RegisterMember([FromBody] MemberRecord memberRecord)
        {
            if (memberRecord == null || string.IsNullOrWhiteSpace(memberRecord.Name))
            {
                var error = "Invalid member data.";
                return BadRequest(new { error });
            }

            var addedMember = _service.RegisterMember(memberRecord.Name);
            return CreatedAtAction(nameof(GetMembers), new { id = addedMember.Id }, addedMember);
        }

        [HttpPost("items/borrow")]
        public IActionResult BorrowItem([FromBody] BorrowItemRecord borrowRecord)
        {
            if (borrowRecord.MemberId <= 0 || borrowRecord.ItemId <= 0)
            {
                var error = "Invalid [member/item] ID data.";
                return BadRequest(new { error });
            }

            var result = _service.BorrowItem(borrowRecord.MemberId, borrowRecord.ItemId, out var message);

            if (!result)
            {
                return Conflict(new { message });
            }

            return Ok(new { message });
        }

        [HttpPost("items/return")]
        public IActionResult ReturnItem([FromBody] ReturnItemRecord returnRecord)
        {
            if (returnRecord.MemberId <= 0 || returnRecord.ItemId <= 0)
            {
                var error = "Invalid [member/item] ID data.";
                return BadRequest(new { error });
            }

            var result = _service.ReturnItem(returnRecord.MemberId, returnRecord.ItemId, out var message);

            if (!result)
            {
                return Conflict(new { message });
            }

            return Ok(new { message });
        }

        #endregion POST
    }
}
