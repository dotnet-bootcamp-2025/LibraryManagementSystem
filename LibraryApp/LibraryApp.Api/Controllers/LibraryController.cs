using Microsoft.AspNetCore.Mvc;
using LibraryApp.Domain;
using LibraryApp.Api.Dtos;
using LibraryApp.Application.Abstractions;

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
            Console.WriteLine($"Controller created with service instance: {_service.GetHashCode()}");
        }

        #region
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
            var members = _service.GetAllMembersWithBorrowStatus();
            return Ok(members);

        }
        #endregion GETs

        #region POSTs
        [HttpPost("book")]
        public IActionResult AddBook([FromBody] AddBookRequest book)
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
        #endregion
        

        // TODO: Borrow an item
        [HttpPost("borrow")]
        public IActionResult BorrowItem([FromBody] BorrowRequest request)
        {
            if (request == null) return BadRequest("Missing data.");
            var ok = _service.BorrowItem(request.MemberId, request.ItemId, out var msg);
            Console.WriteLine($"POST - Borrow Item successfully. MemberId: {request.MemberId}, ItemId: {request.ItemId}");
            if (ok) return Ok(new { success = ok, message = msg });
            return BadRequest(new { success = ok, message = msg });
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
                return CreatedAtAction(nameof(RegisterMember), new { id = addedMember.Id }, addedMember);

            }

        [HttpGet("memberbyid/{memberId}")]
        public IActionResult GetBorrowedItemsByMemberId(int memberId)
        {
            try
            {
                var borrowedItems = _service.GetBorrowedItemsByMemberId(memberId);

                if (borrowedItems == null || !borrowedItems.Any())
                {
                    return NotFound(new
                    {
                        success = false,
                        message = $"No borrowed items found for memberId {memberId}."
                    });
                }

                return Ok(new
                {
                    success = true,
                    memberId,
                    borrowedItems
                });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }


    }
}