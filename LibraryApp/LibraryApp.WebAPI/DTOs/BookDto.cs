using System.ComponentModel.DataAnnotations;

namespace LibraryApp.WebAPI.DTOs
{
    public record BookDto
    {
        [Required(ErrorMessage = "Title is required.")]
        public required string Title { get; init; }

        [Required(ErrorMessage = "Author is required.")]
        public required string Author { get; init; }

        [Required(ErrorMessage = "Pages is required.")]
        public int Pages { get; init; }
    }
}