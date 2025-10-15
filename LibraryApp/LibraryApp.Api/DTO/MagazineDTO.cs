namespace LibraryApp.Api.DTO
{
    public record MagazineDTO
    {
        public string Title { get; set; }

        public int IssueNumber { get; set; }

        public string Publisher { get; set; }
    }
}
