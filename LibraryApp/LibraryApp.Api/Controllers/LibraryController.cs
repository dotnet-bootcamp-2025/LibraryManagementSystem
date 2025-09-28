using LibraryApp.Api.Dtos;
using LibraryApp.Domain;
using LibraryApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Api.Controllers
{
    public class LibraryController : ControllerBase
    {
       // private readonly LibraryService _service = new LibraryService();
        private readonly ILibraryService _service;
        
        public LibraryController(ILibraryService libraryService)
        {
            _service = libraryService;
            _service.Seed(); // Seed data for demo purposes
        }

        //add GET to list all library items
        [HttpGet("items")]  
        public IActionResult GetItems()
        {
            
            var items = _service.Items;
            return Ok(items);
        }

        [HttpPost("add-book")]
        public IActionResult AddBook([FromBody] AddBookRequest request)
        {
            // 1. Validate the request data.
            if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Author))
            {
                // Return a 400 Bad Request if the required fields are missing.
                return BadRequest("Title and Author are required.");
            }

            // 2. Call the service to add the book.
            var newBook = _service.AddBook(request.Title, request.Author, request.Pages);

            // 3. Return a 201 Created status code with the newly created book.
            // The Url.Action method helps generate the URL for the created resource.
            return CreatedAtAction(nameof(GetItems), new { id = newBook.Id }, newBook);
        }

        [HttpGet("search")]
        public IActionResult SearchItems([FromQuery] string? term)
        {
            // 1. El parámetro 'term' es opcional y viene de la URL (e.g., /library/search?term=clean)

            // 2. Llama al método FindItems del servicio.
            var results = _service.FindItems(term);

            // 3. Devuelve los resultados con un código 200 OK.
            if (results == null || !results.Any())
            {
                // Es buena práctica devolver 200 OK con una lista vacía, no 404,
                // si la búsqueda no encuentra resultados.
                return Ok(Array.Empty<LibraryItem>());
            }

            return Ok(results);
        }

        [HttpPost("add-magazine")]
        public IActionResult AddMagazine([FromBody] AddMagazineRequest request)
        {
            // 1. Validate the request data.
            if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Publisher))
            {
                return BadRequest("Title and Publisher are required.");
            }

            // Basic validation for issue number
            if (request.IssueNumber <= 0)
            {
                return BadRequest("Issue number must be greater than zero.");
            }

            // 2. Call the service to add the magazine.
            var newMagazine = _service.AddMagazine(
                request.Title,
                request.IssueNumber,
                request.Publisher
            );

            // 3. Return a 201 Created status code with the new resource details.
            return CreatedAtAction(nameof(GetItems), new { id = newMagazine.Id }, newMagazine);
        }

        [HttpGet("members")]
        public IActionResult GetMembers()
        {

            var members = _service.Members;
            return Ok(members);
        }

        [HttpPost("register-member")]
        public IActionResult RegisterMember([FromBody] RegisterMemberRequest request)
        {
            // 1. Validar la solicitud.
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Member name is required.");
            }

            // 2. Llamar al servicio para registrar al miembro.
            var newMember = _service.RegisterMember(request.Name.Trim());

            // 3. Devolver un 201 Created con el nuevo recurso.
            // Para obtener el recurso creado, podrías usar GetMembers y filtrar, 
            // pero para esta demo, usamos Created con el objeto.
            return CreatedAtAction(nameof(GetMembers), new { id = newMember.Id }, newMember);
        }

        [HttpPost("borrow")]
        public IActionResult BorrowItem([FromBody] BorrowItemRequest request)
        {
            // **1. NUEVA VALIDACIÓN: Verifica si el objeto request completo es nulo.**
            if (request is null)
            {
                return BadRequest("Both MemberId and ItemId are required.");
            }

            // 2. Validación de las propiedades (IDs).
            // Nota: Como ya sabes que 'request' no es null, ahora puedes acceder a sus propiedades.
            if (request.MemberId is null ||
                request.ItemId is null ||
                request.MemberId <= 0 ||
                request.ItemId <= 0)
            {
                return BadRequest("Both MemberId and ItemId must be positive integers.");
            }

            // 3. Llamar al servicio usando .Value (seguro porque ya validaste que no es null).
            string message;
            var success = _service.BorrowItem(request.MemberId.Value, request.ItemId.Value, out message);

            // 4. Devolver la respuesta.
            if (success)
            {
                return Ok(new { Message = message });
            }
            else
            {
                return BadRequest(new { Error = message });
            }
        }

        [HttpPost("return")]
        public IActionResult ReturnItem([FromBody] BorrowItemRequest request)
        {
            // 1. Validar si el cuerpo de la solicitud (request) es nulo.
            if (request is null)
            {
                return BadRequest("Both MemberId and ItemId are required.");
            }

            // 2. Validar que los IDs existan y sean positivos.
            if (request.MemberId is null ||
                request.ItemId is null ||
                request.MemberId <= 0 ||
                request.ItemId <= 0)
            {
                return BadRequest("Both MemberId and ItemId must be positive integers.");
            }

            // 3. Llamar al servicio para devolver el artículo.
            string message;
            // Usamos .Value para convertir de int? a int (seguro después de la validación)
            var success = _service.ReturnItem(request.MemberId.Value, request.ItemId.Value, out message);

            // 4. Devolver la respuesta.
            if (success)
            {
                // 200 OK para una acción de modificación de estado exitosa.
                return Ok(new { Message = message });
            }
            else
            {
                // 400 Bad Request si la devolución falla (ej. artículo no estaba prestado).
                return BadRequest(new { Error = message });
            }
        }

        //TODO: Add more endpoints for other operations (e.g., add magazine, register member, borrow item, return item).
    }
}
