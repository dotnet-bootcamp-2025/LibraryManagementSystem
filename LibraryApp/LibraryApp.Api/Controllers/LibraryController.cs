using LibraryApp.Api.DTOs;
using LibraryApp.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Api.Controllers
{
    public class LibraryController : ControllerBase
    {
        //private static readonly LibraryService _service = new(); // MALA PRÁCTICA, CREA DEPENDENCIAS
        private readonly ILibraryService _service;

        public LibraryController(ILibraryService libraryService)
        {
            _service = libraryService;
        }
        // 1) ListItems
        // Add GET to list all library items
        [HttpGet("items")]
        public IActionResult GetItems()
        {
            var items = _service.GetAllLibraryItems();
            Console.WriteLine($"GET - Service instance: {_service.GetHashCode()}, Items count: {items.Count()}");
            return Ok(items);
        }

        // 3)  AddBook
        // Add POST to add a new book
        [HttpPost("books")]
        public IActionResult AddBook([FromBody] BookDto book)
        {
            if (book == null || string.IsNullOrEmpty(book.Title) || string.IsNullOrEmpty(book.Author))
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

        // 4) AddMagazine
        // Add POST to add a new magazine
        [HttpPost("magazine")]
        public IActionResult addMagazine([FromBody] MagazineDto mag)
        {
            if (mag == null || string.IsNullOrEmpty(mag.Title) || string.IsNullOrEmpty(mag.Publisher))
            {
                return BadRequest("Invalid magazine data.");
            }

            var items = _service.GetAllLibraryItems();

            Console.WriteLine($"POST - Service instance: {_service.GetHashCode()}, Items count before: {items.Count()}");

            var addedMag = _service.AddMagazine(mag.Title, mag.IssueNumber, mag.Publisher);

            items = _service.GetAllLibraryItems();

            Console.WriteLine($"POST - Items count after: {items.Count()}");

            return CreatedAtAction(nameof(GetItems), new { id = addedMag.Id }, addedMag);
        }

        // 5) ListMembers
        // Add GET to list all members
        //[HttpGet("members")]
        //public IActionResult GetMembers()
        //{
        //    var members = _service.Members;
        //    return Ok(members);
        //}

        // 6) RegisterMember
        // Add POST to register a new member
        [HttpPost("member")]
        public IActionResult registerMember([FromBody] MemberDto member)
        {
            if (member == null || string.IsNullOrEmpty(member.Name))
            {
                return BadRequest("Invalid member data.");
            }
            var addedMember = _service.RegisterMember(member.Name);
            return CreatedAtAction(nameof(GetItems), new { id = addedMember.Id }, addedMember);
        }

        // 7) BorrowItem
        // Add POST to borrow an item
        [HttpPost("borrow")]
        public IActionResult borrowItem([FromBody] BorrowReturnDto itemId)
        {
            if (itemId == null || itemId.MemberId <= 0 || itemId.ItemId <= 0)
            {
                return BadRequest("Invalid borrow data.");
            }
            var success = _service.BorrowItem(itemId.MemberId, itemId.ItemId, out string message);
            if (success)
            {
                return Ok(message);
            }
            else
            {
                return BadRequest(message);
            }
        }

        // 8) ReturnItem
        // Add POST to return an item
        //[HttpPost("return")]
        //public IActionResult returnItem([FromBody] BorrowReturnDto itemId)
        //{
        //    if (itemId == null || itemId.MemberId <= 0 || itemId.ItemId <= 0)
        //    {
        //        return BadRequest("Invalid borrow data.");
        //    }
        //    var success = _service.ReturnItem(itemId.MemberId, itemId.ItemId, out string message);
        //    if (success)
        //    {
        //        return Ok(message);
        //    }
        //    else
        //    {
        //        return BadRequest(message);
        //    }
        //}
    }
}
