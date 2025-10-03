using Microsoft.AspNetCore.Mvc;
using LibraryApp.Services;
using LibraryApp.Domain;
using LibraryApp.Api.Dtos;

namespace LibraryApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class LibraryController : ControllerBase
    {
        // OLD approach --> private readonly LibraryService _service = new();
        private readonly ILibraryService _service;
        public LibraryController(ILibraryService libraryService)
        {
            //_service = (LibraryService)libraryService;
            _service = libraryService;
        }

        // Add GET to list all library items
        [HttpGet("items")]
        public IActionResult GetItems()
        {
            _service.Seed();
            var items = _service.Items;
            return Ok(items);
        }

        // List all members
        [HttpGet("members")]
        public IActionResult GetMembers()
        {
            var members = _service.Members;
            return Ok(members);
        }

        //Add POST to add a new book

        [HttpPost("book")]
        public IActionResult AddBook([FromBody] AddBookRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Author))
            {
                return BadRequest("Invalid book data.");
            }

            var addedBook = _service.AddBook(request.Title, request.Author, request.Pages);

            return CreatedAtAction(nameof(GetItems), new { id = addedBook.Id }, addedBook);
        }

        // TODO: Borrow an item
        [HttpPost("borrow")]
        public IActionResult BorrowItem([FromBody] BorrowRequest request)
        {
            if (request == null || request.MemberId <= 0 || request.ItemId <= 0)
            {
                return BadRequest("Invalid borrow request.");
            }

            if (_service.BorrowItem(request.MemberId, request.ItemId, out var message))
            {
                return Ok(new { Success = true, Message = message });
            }
            else
            {
                return BadRequest(new { Success = false, Message = message });
            }
        }

        // TODO: Return an item
        [HttpPost("return")]
        public IActionResult ReturnItem([FromBody] ReturnRequest request)
        {
            if (request == null || request.MemberId <= 0 || request.ItemId <= 0)
            {
                return BadRequest("Invalid return request.");
            }

            if (_service.ReturnItem(request.MemberId, request.ItemId, out var message))
            {
                return Ok(new { Success = true, Message = message });
            }
            else
            {
                return BadRequest(new { Success = false, Message = message });
            }
        }

        // TODO: Add a new magazine
        [HttpPost("magazine")]
        public IActionResult AddMagazine([FromBody] AddMagazineRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Title) ||
                request.IssueNumber <= 0 || string.IsNullOrWhiteSpace(request.Publisher))
            {
                return BadRequest("Invalid magazine data.");
            }

            var addedMagazine = _service.AddMagazine(request.Title, request.IssueNumber, request.Publisher);
            return CreatedAtAction(nameof(GetItems), new { id = addedMagazine.Id }, addedMagazine);
        }


        // TODO: Find Items
        [HttpPost("find")]
        public IActionResult FindItems([FromBody] FindItemsRequest request)
        {
            var items = _service.FindItems(request.Term);
            return Ok(items);
        }

        // TODO: Register a new member
        [HttpPost("member")]
        public IActionResult RegisterMember([FromBody] RegisterMemberRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Invalid member data.");
            }

            var addedMember = _service.RegisterMember(request.Name);
            return CreatedAtAction(nameof(GetMembers), new { id = addedMember.Id }, addedMember);
        }
    }
}