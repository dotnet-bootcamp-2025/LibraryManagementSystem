namespace LibraryApp.Domain.Entities
{
    public class BorrowedItem
    {
        public int Id { get; set; }
        public int LibraryItemId { get; set; }
        public int MemberId { get; set; }

        //otras entities
        public Member? Member { get; set; }
        public LibraryItem? LibraryItem { get; set; }
    }
}
