namespace LibraryApp.Domain.Entities
{
    public class BorrowedItem
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int LibraryItemId { get; set; }
        public DateTime BorrowedDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public Member? Member { get; set; }
        public LibraryItem? LibraryItem { get; set; }
    }
}