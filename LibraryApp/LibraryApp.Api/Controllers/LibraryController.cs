using Microsoft.AspNetCore.Mvc;
using LibraryApp.Domain;
using LibraryApp.Api.Dtos;
using LibraryApp.Application.Abstractions;

namespace LibraryApp.Api.Controllers
{
    public class LibraryController : ControllerBase
    {
        //OLD approach -> private readonly LibraryService _service = new();
        private readonly ILibraryService _service;

        public LibraryController(ILibraryService libraryService)
        {
            _service = libraryService;
            Console.WriteLine($"Controller created with server instance: {_service.GetHashCode()}");
            //_service.Seed();  Seed data for demonstration
        }

        // Add GET to list all library items
        [HttpGet("items")]
        public IActionResult GetItems()
        {
            var items = _service.GetAllLibraryItems();
            Console.WriteLine($"GetItems called, returning{_service.GetHashCode()}, {items.Count()} items.");
            return Ok(items);
        }

        //Add GET to list all members, this is an extra endpoint
        [HttpGet("members")]
        public IActionResult GetMembers()
        {
            var members = _service.GetAllMembers();
            return Ok(members);
        }

        // Homework: Add POST to add a new book
        [HttpPost("books")]
        public IActionResult AddBook([FromBody] BookDTO bookDto)
        {
            if (bookDto == null || string.IsNullOrWhiteSpace(bookDto.Title) || string.IsNullOrWhiteSpace(bookDto.Author))
            {
                return BadRequest("Invalid book data.");
            }
            var item = _service.GetAllLibraryItems();

            Console.WriteLine($"AddBook called, current items count: {item.Count()}");
            var book = _service.AddBook(bookDto.Title, bookDto.Author, bookDto.Pages);

            item = _service.GetAllLibraryItems();

            Console.WriteLine($"Book added with ID: {book.Id}, new items count: {_service.GetAllLibraryItems().Count()}");
            return CreatedAtAction(nameof(GetItems), new { id = book.Id }, book);
        }

        // TODO : Add more endpoints for magazines, members, borrowing and returning items

        // Add POST to add a new magazine
        [HttpPost("magazines")]
        public IActionResult AddMagazine([FromBody] MagazineDTO magDto)
        {
            if (magDto == null || string.IsNullOrWhiteSpace(magDto.Title) || string.IsNullOrWhiteSpace(magDto.Publisher) || magDto.IssueNumber <= 0)
            {
                return BadRequest("Invalid magazine data.");
            }

            var item = _service.GetAllLibraryItems();
            Console.WriteLine($"AddMagazine called, current items count: {item.Count()}");
            var magazine = _service.AddMagazine(magDto.Title, magDto.IssueNumber, magDto.Publisher);

            item = _service.GetAllLibraryItems();

            Console.WriteLine($"Magazine added with ID: {magazine.Id}, new items count: {_service.GetAllLibraryItems().Count()}");
            return CreatedAtAction(nameof(GetItems), new { id = magazine.Id }, magazine);
        }

        //// Add POST to register a new member
        [HttpPost("members")]
        public IActionResult registermember([FromBody] MemberDTO memberdto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var item = _service.RegisterMember;
            Console.WriteLine($"RegisterMember called, current items count: {item.ToString}");
            var member = _service.RegisterMember(memberdto.Name);

            item = _service.RegisterMember;

            Console.WriteLine($"Member added with ID: {member.Id}, new items count: {_service.RegisterMember}");
            return CreatedAtAction(nameof(GetItems), new { id = member.Id }, member);
        }

        //// Add PATCH to borrow an item
        [HttpPatch("borrow")]
        public IActionResult BorrowItem(int memberId, int itemId)
        {
            if (_service.BorrowItem(memberId, itemId, out string message))
            {
                return Ok(message);
            }
            return BadRequest(message);
        }

        // Add PATCH to return an item
        [HttpPatch("return")]
        public IActionResult ReturnItem(int memberId, int itemId)
        {
            if (_service.ReturnItem(memberId, itemId, out string message))
            {
                return Ok(message);
            }
            return BadRequest(message);
        }

        //Add GET to search items by term
        [HttpGet("search")]
        public IActionResult SearchItems(string bookname)
        {
            var items = _service.FindItems(bookname);
            return Ok(items);
        }

        [HttpGet("ItemsByMember")]
        public IActionResult GetMembersWithItemsBorrowed()
        {
            var members = _service.GetAllMembers();

            var result = members.Select(m => new MemberWithBorrowItemDTO
            {
                MemberId = m.Id,
                Name = m.Name,
                BorrowedTitles = _service.GetAllLibraryItems()
                .Where(li => li.BorrowedByMemberId == m.Id && li.IsBorrowed)
                .Select(li => li.Title ?? string.Empty)
                .ToList()
            }).ToList();
            return Ok(result);
        }

    }
}
