using System.ComponentModel.DataAnnotations;

namespace LibraryApp.WebAPI.DTOs
{
    public record MagazineDto
    {
        [Required]
        public required string Title { get; init; }

        [Required(ErrorMessage = "Publisher is required.")]
        public required string Publisher { get; init; }

        [Required(ErrorMessage = "IssueNumber is required.")]
        public int IssueNumber { get; init; }
    }
}