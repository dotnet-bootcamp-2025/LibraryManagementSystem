using LibraryApp.Domain;
using LibraryApp.Services;
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
            _service.Seed();
        }

        #region GET

        //OPTIONAL EXTRA GET RESPONSES

        //[HttpGet("items")]
        //public IActionResult GetItems()
        //{
        //    var items = _service.Items;
        //    return Ok(items);
        //}

        //[HttpGet("items/{term}")]
        //public IActionResult GetSearchItems(string term)
        //{
        //    var items = _service.FindItems(term);
        //    return Ok(items);
        //}

        [HttpGet("items")]
        public IActionResult GetItems([FromQuery] string? term)
        {
            var items = _service.FindItems(term);
            return Ok(items);
        }

        [HttpGet("members")]
        public IActionResult GetMembers()
        {
            var items = _service.Members;
            return Ok(items);
        }

        #endregion GET

        #region POST

        [HttpPost("books")]
        public IActionResult AddBook([FromBody] BookRecord bookRecord)
        {
            if (bookRecord == null || string.IsNullOrWhiteSpace(bookRecord.Title) 
                || string.IsNullOrWhiteSpace(bookRecord.Author))
            {
                return BadRequest("Invalid book data.");
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
                return BadRequest("Invalid magazine data.");
            }

            var addedMag = _service.AddMagazine(magRecord.Title, magRecord.IssueNumber, magRecord.Publisher);
            return CreatedAtAction(nameof(GetItems), new { id = addedMag.Id }, addedMag);
        }

        [HttpPost("members")]
        public IActionResult RegisterMember([FromBody] MemberRecord memberRecord)
        {
            if (memberRecord == null || string.IsNullOrWhiteSpace(memberRecord.Name))
            {
                return BadRequest("Invalid member data.");
            }

            var addedMember = _service.RegisterMember(memberRecord.Name);
            return CreatedAtAction(nameof(GetMembers), new { id = addedMember.Id }, addedMember);
        }

        #endregion POST

        #region PATCH

        [HttpPatch("items/{itemId}/borrow")]
        public IActionResult BorrowItem(int itemId, [FromBody] int memberId)
        {
            if (memberId <= 0 || itemId <= 0)
            {
                return BadRequest("Invalid [member/item] id data.");
            }

            var message = string.Empty;
            var result = _service.BorrowItem(memberId, itemId, out message);

            if (!result)
            {
                return Conflict(message);
            }

            return Ok(message);
        }

        [HttpPatch("items/{itemId}/return")]
        public IActionResult ReturnItem(int itemId, [FromBody] int memberId)
        {
            if (memberId <= 0 || itemId <= 0)
            {
                return BadRequest("Invalid [member/item] id data.");
            }

            var message = string.Empty;
            var result = _service.ReturnItem(memberId, itemId, out message);

            if (!result)
            {
                return Conflict(message);
            }

            return Ok(message);
        }

        #endregion PATCH
    }
}
