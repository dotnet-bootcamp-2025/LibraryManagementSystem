namespace LibraryApp.Application.DTOs
{
    public class LoanDetailsDto
    {
        public required string ItemTitle { get; set; }
        public string? BorrowedDate { get; set; }
        public string? ReturnDate { get; set; }
        public bool IsExpired { get; set; }
    }
}
