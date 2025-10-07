using LibraryApp.Api.Dtos;
using LibraryApp.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;
namespace LibraryApp.Api.Controllers
{
    public class LibraryController : ControllerBase
    {
        private readonly ILibraryService _service;
        public LibraryController(ILibraryService libraryService)
        {
            _service = libraryService;
            System.Console.WriteLine($"Controller created with service instance: {_service.GetHashCode()}");
        }
        // Add GET to list all library items
        [HttpGet("items")]
        public IActionResult GetItems()
        {
            var items = _service.GetAllLibraryItems();
            System.Console.WriteLine($"GET - Service instance: {_service.GetHashCode()}, Items count: {items.Count()}");
            return Ok(items);
        }

        [HttpGet("items/{id}")]
        public IActionResult GetItem(int id)
        {
            var item = _service.GetAllLibraryItems().FirstOrDefault(x => x.Id == id);
            if (item == null)
                {
                    System.Console.WriteLine($"GET /items/{id} - Not Found");
                return NotFound();
                }
            System.Console.WriteLine($"GET - Service instance: {_service.GetHashCode()}, Item: {item.Id}");
            return Ok(item);
        }
        
        // Add Post to add a new book
        [HttpPost("books")]
        public IActionResult AddBook([FromBody] AddBookRequest book)
        {
            if (book == null || string.IsNullOrWhiteSpace(book.Title) || string.IsNullOrWhiteSpace(book.Author))
            {
                return BadRequest("Invalid book data.");
            }
            var items = _service.GetAllLibraryItems();
            System.Console.WriteLine($"POST - Service instance: {_service.GetHashCode()}, Items count before: {items.Count()}");
            var addedBook = _service.AddBook(book.Title, book.Author, book.Pages);
            items = _service.GetAllLibraryItems();
            System.Console.WriteLine($"POST - Items count after: {items.Count()}");
            return CreatedAtAction(nameof(GetItems), new { id = addedBook.Id }, addedBook);
        }
        // Search items by title
        [HttpGet("items/search")]
        public IActionResult SearchItems([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return BadRequest("Invalid term data.");
            }
            var items = _service.FindItems(term);
            System.Console.WriteLine($"GET /items/search - term='{term}' matched: {items.Count()}");
            return Ok(items);
        }
        // Add Post to add a new magazine
        [HttpPost("magazines")]
        public IActionResult AddMagazine([FromBody]AddMagazineRequest magazine)
        {
            if (magazine == null || string.IsNullOrWhiteSpace(magazine.Title)
                || string.IsNullOrWhiteSpace(magazine.Publisher))
            {
                return BadRequest("Invalid magazine data.");
            }
            var items = _service.GetAllLibraryItems();
            System.Console.WriteLine($"POST - Service instance: {_service.GetHashCode()}, Items count before: {items.Count()}");
            var addedMagazine = _service.AddMagazine(magazine.Title, magazine.IssueNumber, magazine.Publisher);
            System.Console.WriteLine($"POST - Items count after: {items.Count()}");
            return CreatedAtAction(nameof(GetItems), new { id = addedMagazine.Id }, addedMagazine);
        }
        //Post to borrow an item
       [HttpPost("borrowItem")]
        public IActionResult BorrowItem([FromBody] BorrowRequest dto)
        {
            if (dto == null) return BadRequest("Missing data.");
            var ok = _service.BorrowItem(dto.MemberId, dto.ItemId, out var msg);
            System.Console.WriteLine($"POST - Borrow Item successfully. MemberId: {dto.MemberId}, ItemId: {dto.ItemId}");
            
            if (ok) return Ok(new { success = ok, message = msg });
            return BadRequest(new { success = ok, message = msg });
            
        }
        // Post to return an item
        [HttpPost("returnItem")]
        public IActionResult ReturnItem([FromBody] BorrowRequest dto)
        {
            if (dto == null) return BadRequest("Missing data.");
            var ok = _service.ReturnItem(dto.MemberId, dto.ItemId, out var msg);
            System.Console.WriteLine($"POST - Return Item successfully. MemberId: {dto.MemberId}, ItemId: {dto.ItemId}");
            if (ok) return Ok(new { success = ok, message = msg });
            return BadRequest(new { success = ok, message = msg });
        }
        // Get all registered members
        [HttpGet("members")]
        public IActionResult ListMembers()
        {
            var members = _service.GetAllMembers();
            System.Console.WriteLine($"GET - Service instance: {_service.GetHashCode()}, List Members");
            return Ok(members);
        }
        // Register a new member
        [HttpPost("member")]
        public IActionResult RegisterMember([FromBody] RegisterMemberRequest dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest("Invalid name.");
            }
            var members = _service.GetAllMembers();
            System.Console.WriteLine($"POST - Service instance: {_service.GetHashCode()}, Adding a Member: {dto.Name},members count before: {members.Count()}");
            var added = _service.RegisterMember(dto.Name);
            System.Console.WriteLine($"POST - Service instance: {_service.GetHashCode()}, Member added: {dto.Name},members count after: {members.Count()}");
            return CreatedAtAction(nameof(ListMembers), new { id = added.Id }, added);
        }

        [HttpGet("members/{id}")]
        public IActionResult GetMember(int id)
        {
            var member = _service.GetAllMembers().FirstOrDefault(m => m.Id == id);
            if (member == null)
            {
                System.Console.WriteLine("Member not found.");
                return NotFound();
            }
            return Ok(member);
        }
    }
}