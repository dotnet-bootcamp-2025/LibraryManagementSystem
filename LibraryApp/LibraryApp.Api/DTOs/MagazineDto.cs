namespace LibraryApp.Api.DTOs
{
    public class MagazineDto
    {
        public required string Title { get; set; }
        public int IssueNumber { get; set; }
        public required string Publisher { get; set; }
    }
}