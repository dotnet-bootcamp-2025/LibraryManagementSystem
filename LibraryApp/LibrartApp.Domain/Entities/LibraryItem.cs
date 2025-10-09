namespace LibrartApp.Domain.Entities
{
    public class LibraryItem
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public bool IsBorrowed { get; set; }
        public string? Author { get; set; }
        public int? Pages { get; set; }
        public int? IssueNumber { get; set; }
        public string? Publisher { get; set; }

        public int Type { get; set; }

        public List<BorrowItem> BorrowItems { get; set; } = new();


    }
}
