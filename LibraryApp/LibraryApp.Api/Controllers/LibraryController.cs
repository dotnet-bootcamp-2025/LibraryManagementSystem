using LibraryApp.Api.DTOs;
using LibraryApp.Application.Abstractions;
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
            Console.WriteLine($"Controller created with service instance: {_service.GetHashCode()}");
        }

        #region GETs
        // Add GET to list all library items
        [HttpGet("items")]
        public IActionResult GetItems()
        {
            var items = _service.GetAllLibraryItems();
            Console.WriteLine($"GET - Service instance: {_service.GetHashCode()}, Items count: {items.Count()}");
            return Ok(items);
        }

        [HttpGet("members")]
        public IActionResult GetMembers()
        {
            var members = _service.GetAllMembers();
            Console.WriteLine($"GET - Service instance: {_service.GetHashCode()}, Members count: {members.Count()}");
            return Ok(members);
        }
        [HttpGet("borrowItems")]
        public IActionResult GetBorrowedItems([FromQuery] int memberId)
        {
            var borrowedItems = _service.GetBorrowedItemsFromMember(memberId);
            Console.WriteLine($"GET - MemberId: {memberId}, BorrowedItems count: {borrowedItems.Count()}");
            return Ok(borrowedItems);
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
            var item = _service.GetAllLibraryItems();
            Console.WriteLine($"POST - Service instance: {_service.GetHashCode()}, Item count before: {item.Count()}");
            var addedBook = _service.AddBook(book.Title, book.Author, book.Pages);
            item = _service.GetAllLibraryItems();
            Console.WriteLine($"POST - Item count after: {item.Count()}");
            return CreatedAtAction(nameof(GetItems), new { id = addedBook.Id }, addedBook);
        }

        [HttpPost("magazines")]
        public IActionResult AddMagazine([FromBody] CreateMagazineRequest magazine)
        {
            if (magazine == null || string.IsNullOrWhiteSpace(magazine.Title) || string.IsNullOrWhiteSpace(magazine.Publisher))
            {
                return BadRequest("Invalid magazine data.");
            }
            var item = _service.GetAllLibraryItems();
            Console.WriteLine($"POST - Service instance: {_service.GetHashCode()}, Item count before: {item.Count()}");
            var addedMagazine = _service.AddMagazine(magazine.Title, magazine.IssueNumber, magazine.Publisher);
            Console.WriteLine($"POST - Item count after: {item.Count()}");
            return CreatedAtAction(nameof(GetItems), new { id = addedMagazine.Id }, addedMagazine);
        }

        [HttpPost("members")]
        public IActionResult RegisterMember([FromBody] CreateMemberRequest member)
        {
            if (member == null || string.IsNullOrWhiteSpace(member.Name))
            {
                return BadRequest("Invalid member data.");
            }
            var memb = _service.GetAllMembers();
            Console.WriteLine($"POST - Service instance: {_service.GetHashCode()}, Member count before: {memb.Count()}");
            var addedMember = _service.RegisterMember(member.Name);
            Console.WriteLine($"POST - Member count after: {memb.Count()}");
            return CreatedAtAction(nameof(GetMembers), new { id = addedMember.Id }, addedMember);
        }

        [HttpPost("borrow")]
        public IActionResult BorrowItem([FromBody] CreateBorrowRequest borrow)
        {
            if (borrow == null) return BadRequest("Missing data.");

            var ok = _service.BorrowItem(borrow.memberId, borrow.itemId, out var msg);
            //Console.WriteLine($"POST - Borrow Item successfully. MemberId = {borrow.memberId}, ItemId = {borrow.itemId}");

            if (ok) return Ok(new {success = ok, message = msg});
            return BadRequest(new {success = ok, message = msg});
        }

        [HttpPost("return")]
        public IActionResult ReturnItem([FromBody] CreateBorrowRequest borrow)
        {
            if (borrow == null) return BadRequest("Missing data.");

            var ok = _service.ReturnItem(borrow.memberId, borrow.itemId, out var msg);
            //Console.WriteLine($"POST - Return Item successfully. MemberId = {borrow.memberId}, ItemId = {borrow.itemId}");

            if (ok) return Ok(new { success = ok, message = msg });
            return BadRequest(new { success = ok, message = msg });
        }
        #endregion POSTs
    }
}
