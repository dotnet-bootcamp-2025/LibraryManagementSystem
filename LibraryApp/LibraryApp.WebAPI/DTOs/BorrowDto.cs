using System.ComponentModel.DataAnnotations;

namespace LibraryApp.WebAPI.DTOs
{
    public record BorrowDto
    {
        [Required]
        public int MemberId { get; init; }

        [Required]
        public int ItemId { get; init; }
    }
}