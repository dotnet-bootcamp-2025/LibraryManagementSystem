namespace LibraryApp.Api.DTOs
{
    public class BorrowedItemDTO
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int LibraryItemId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string BorrowedDate { get; set; } = string.Empty; // MM/dd/yyyy
        public string DueDate { get; set; } = string.Empty;      // MM/dd/yyyy
        public bool IsActive { get; set; }
    }
}
