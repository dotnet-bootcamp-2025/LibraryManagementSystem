using Microsoft.AspNetCore.Mvc;
using LibraryApp.Api.DTOs;
using LibraryApp.Application.Abstraction;

namespace LibraryApp.Api.Controllers
{
    public class LibraryController : ControllerBase
    {
        //OLD private static readonly LibraryService _service = new();
        private readonly ILibraryService _service;

        public LibraryController(ILibraryService LibraryService )
        {
            _service = LibraryService;
            Console.WriteLine("LibraryController instantiated.");
            { _service.GetHashCode(); } // Just to use _service and avoid warnings
        }

        //Add GET to list all library items
        [HttpGet("items")]
        public IActionResult GetItems()
        {
            var items = _service.GetAllLibraryItems();
            Console.WriteLine($"GET - ServiceCollection INSTANCE : {_service.GetHashCode()}, Items Count: {items.Count()}");
            return Ok(items);
        }

        //[HttpGet("search")]
        //public IActionResult SearchItems([FromQuery] string term)
        //{
        //    var results = _service.FindItems(term);
        //    return Ok(results);
        //}

        //// Homework:
        //// Add Post to add a new book
        [HttpPost("books")]
        public IActionResult AddBook([FromBody] AddBookDTO book)
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

        ////TODO : Add more endpoints for magazines, members, borrowing, and returning items.

        [HttpPost("magazines")]
        public IActionResult AddMagazine([FromBody] AddMagazineDTO magazine)
        {
            if (magazine == null || string.IsNullOrWhiteSpace(magazine.Title) || magazine.IssueNumber <= 0 || string.IsNullOrWhiteSpace(magazine.Publisher))
            {
                return BadRequest("Invalid magazine data.");
            }
            var items = _service.GetAllLibraryItems();
            Console.WriteLine($"POST - Service instance: {_service.GetHashCode()}, Items count before: {items.Count()}");
            var addedMagazine = _service.AddMagazine(magazine.Title, magazine.IssueNumber, magazine.Publisher);
            items = _service.GetAllLibraryItems();
            return CreatedAtAction(nameof(GetItems), new { id = addedMagazine.Id }, addedMagazine);
        }


        //[HttpGet("members")]
        //public IActionResult GetMembers()
        //{
        //    var members = _service.Members;
        //    return Ok(members);
        //}

        //[HttpPost("members")]
        //public IActionResult RegisterMember([FromBody] RegisterMemberDTO member)
        //{
        //    if (member == null || string.IsNullOrWhiteSpace(member.Name))
        //    {
        //        return BadRequest("Invalid member data.");
        //    }
        //    var newMember = _service.RegisterMember(member.Name);
        //    return CreatedAtAction(nameof(GetMembers), new { id = newMember.Id }, newMember);
        //}

        //[HttpPost("borrow")]
        //public IActionResult BorrowItem([FromBody] BorrowDTO borrowRequest)
        //{
        //    if (borrowRequest == null || borrowRequest.MemberId <= 0 || borrowRequest.ItemId <= 0)
        //    {
        //        return BadRequest("Invalid borrow request data.");
        //    }
        //    if (_service.BorrowItem(borrowRequest.MemberId, borrowRequest.ItemId, out string message))
        //    {
        //        return Ok(message);
        //    }
        //    return BadRequest(message);
        //}

        //[HttpPost("return")]
        //public IActionResult ReturnItem([FromBody] ReturnDTO returnRequest)
        //{
        //    if (returnRequest == null || returnRequest.MemberId <= 0 || returnRequest.ItemId <= 0)
        //    {
        //        return BadRequest("Invalid return request data.");
        //    }
        //    if (_service.ReturnItem(returnRequest.MemberId, returnRequest.ItemId, out string message))
        //    {
        //        return Ok(message);
        //    }
        //    return BadRequest(message);
        //}



    }
}
