using LibraryApp.Api.Records;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain;
using Microsoft.AspNetCore.Mvc;
using static LibraryApp.Api.Records.LibraryController;
using static LibraryApp.Api.Records.MagazineRecord;
using static LibraryApp.Api.Records.AddMemberRecord;
using static LibraryApp.Api.Records.ReturnRecord;

namespace LibraryApp.Api.Controllers
{
    public partial class LibraryController : ControllerBase
    {
        private readonly ILibraryService _service;

        public LibraryController(ILibraryService libraryservice)
        {
            _service = libraryservice;
        }

        [HttpGet("See All Books and Magazines")]
        public IActionResult GetItems()
        {
            var items = _service.GetAllLibraryItems();
            Console.WriteLine($"GET - Service instance: {_service.GetHashCode()}, Items count: {items.Count()}");

            return Ok(items);
        }

        //add POST to add a new book
        [HttpPost("Add Book")]
        public IActionResult AddBook([FromBody] BookRecord Book)
        {
            if (Book == null || string.IsNullOrWhiteSpace(Book.Title) || string.IsNullOrWhiteSpace(Book.Author))
            {
                return BadRequest("Invalid book data.");
            }

            var items = _service.GetAllLibraryItems();
            Console.WriteLine($"POST - Service instance: {_service.GetHashCode()}, Items count before adding: {items.Count()}");

            var addedBook = _service.AddBook(Book.Title, Book.Author, Book.Pages);
            Console.WriteLine($"POST - Added Book after: {items.Count()}");
            return CreatedAtAction(nameof(GetItems), new { id = addedBook.Id }, addedBook);
        }

        [HttpPost("Add Magazine")]
        public IActionResult AddMagazine([FromBody] CreateMagazineRecord Magazine)
        {
            if (Magazine == null || string.IsNullOrWhiteSpace(Magazine.Title) || string.IsNullOrWhiteSpace(Magazine.Publisher))
            {
                return BadRequest("Invalid Magazine data.");
            }
            var items = _service.GetAllLibraryItems();
            Console.WriteLine($"POST - Service instance: {_service.GetHashCode()}, Items count before adding: {items.Count()}");

            var addedMagazine = _service.AddMagazine(Magazine.Title, Magazine.IssueNumber, Magazine.Publisher);
            Console.WriteLine($"POST - Added Book after: {items.Count()}");
            return CreatedAtAction(nameof(GetItems), new { id = addedMagazine.Id }, addedMagazine);
        }

        [HttpPost("Borrow an Item")]

        public IActionResult BorrowItem([FromBody] BorrowRecord borrowRequest)
        {
            if (borrowRequest == null || borrowRequest.memberId <= 0 || borrowRequest.itemId <= 0)
            {
                return BadRequest("Invalid member or item ID.");
            }
            var ok = _service.BorrowItem(borrowRequest.memberId, borrowRequest.itemId, out string message);
            if (ok)
            {
                return Ok(new { success = ok, message = message });
            }
            return BadRequest(new { success = ok, message = message });
        }

        [HttpPatch("Return an Item")]
        public IActionResult ReturnItem([FromBody] BorrowRecord returnRequest)
        {
            if (returnRequest == null || returnRequest.memberId <= 0 || returnRequest.itemId <= 0)
            {
                return BadRequest("Invalid member or item ID.");
            }
            var ok = _service.ReturnItem(returnRequest.memberId, returnRequest.itemId, out string message);
            if (ok)
            {
                return Ok(new { success = ok, message = message });
            }
            return BadRequest(new { success = ok, message = message });
        }

        [HttpGet("See all Members")]
        public IActionResult GetMembers()
        {
            var members = _service.GetAllMembers();
            return Ok(members);
        }

        [HttpPost("Add new Member")]
        public IActionResult RegisterMember([FromBody] AddMemberRecord Member)
        {
            if (Member == null || string.IsNullOrWhiteSpace(Member.Name) || string.IsNullOrWhiteSpace(Member.Name))
            {
                return BadRequest("Invalid Name.");
            }
            var newmember = _service.RegisterMember(Member.Name);
            //here we return a message with the created member
            Console.WriteLine($"Member created: Id={newmember.Id}, Name={newmember.Name}");
            //here we return the created member with its id
            return CreatedAtAction(nameof(GetMembers), new { id = newmember.Id }, newmember);
        }

        [HttpGet("Find Item")]
        public IActionResult FindItems(string? term)
        {
            var items = _service.FindItems(term);

            //here validate if items is empty
            if (!items.Any())
            {
                return NotFound("No items found matching the search term.");
            }
            //if we found items return them
            return Ok(items);
        }

        [HttpGet("All Borrowed Items")]
        public IActionResult GetAllBorrowedItems()
        {
            var items = _service.GetAllBorrowedItems();
            if (!items.Any())
            {
                return NotFound("There are no borrowed items.");
            }
            return Ok(items);
        }

        // add a PATCH to return all active borrowed items of a member, given the member id
        [HttpGet("Return all active borrowed items by Member")]
        public IActionResult ReturnItemsByMember(int memberId)
        {

            string message;
            var result = _service.ReturnAllItemsByMember(memberId, out message);
            if (!result)
            {
                return NotFound(message);
            }
            return Ok(new { success = result, message = message });
        }
    }

}
