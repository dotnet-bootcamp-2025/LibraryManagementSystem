// entities for data base
namespace LibraryApp.Domain.Entities
{
    public class LibraryItems
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public bool IsBorrowed { get; set; }

        public string? Author { get; set; }
        public int? Pages { get; set; }
        public int? IssueNumber { get; set; }
        public string? Publisher { get; set; }

        public int Type { get; set; }

        // Navigation property for borrowed items
        public List<BorrowedItem>? BorrowedItems { get; set; }
    }
}
