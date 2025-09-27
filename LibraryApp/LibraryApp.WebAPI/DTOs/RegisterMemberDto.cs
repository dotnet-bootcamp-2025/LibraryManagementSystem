using System.ComponentModel.DataAnnotations;

namespace LibraryApp.WebAPI.DTOs
{
    public record RegisterMemberDto
    {
        [Required(ErrorMessage = "Name is required.")]
        public required string Name { get; init; }

    }
}
