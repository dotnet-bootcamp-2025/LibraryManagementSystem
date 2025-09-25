using LibraryApp.Services;
using LibraryApp.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ApiLibrary.Controllers
{
    public class LibraryController : ControllerBase
    {
        private readonly ILibraryService _service;

        public LibraryController(ILibraryService libraryService)
        {
            _service = libraryService;
            _service.Seed();
        }

        [HttpGet("items")]
        public IActionResult GetItems()
        {
            var items = _service.Items;

            return Ok(items);
        }

        [HttpGet("items")]
        public IActionResult GetMembers()
        {
            var members = _service.Members;

            return Ok(members);
        }

        [HttpPost("item")]
        public IActionResult AddBook([FromBody] Book book)
        {
            if(book == null || string.IsNullOrWhiteSpace(book.Title) || string.IsNullOrWhiteSpace(book.Author))
            {
                return BadRequest("Invalid book data");
            }
            var addBook = _service.AddBook(book.Title, book.Author, book.Pages);

            return CreatedAtAction(nameof(GetItems), new {  id = addBook.Id, addBook});
        }

        [HttpPost("item")]
        public IActionResult AddMagazzine([FromBody] Magazine magazzine)
        {
            if (magazzine == null || string.IsNullOrWhiteSpace(magazzine.Title) || string.IsNullOrWhiteSpace(magazzine.Publisher) || magazzine.IssueNumber <= 0)
            {
                return BadRequest("Invalid magazzine data");
            }

            var addMagazzine = _service.AddMagazine(magazzine.Title, magazzine.IssueNumber, magazzine.Publisher);

            return CreatedAtAction(nameof(GetItems), new { id = addMagazzine.Id, addMagazzine });
        }

        [HttpPost("item")]
        public IActionResult AddMember([FromBody] Member member)
        {
            if (member == null || string.IsNullOrWhiteSpace(member.Name))
            {
                return BadRequest("Invalid member data");
            }

            var addMember = _service.RegisterMember(member.Name);

            return CreatedAtAction(nameof(GetMembers), new { id = addMember.Id, addMember });
        }

        [HttpPost("items")]
        public IActionResult BorrowItem(int memberId, int itemId)
        {
            if (memberId <= 0 || itemId <= 0)
            {

            }

            return Ok();
        }

    }
}
