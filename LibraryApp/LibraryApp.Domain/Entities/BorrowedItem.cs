//POCO entity class for BorrowedItem

namespace LibraryApp.Domain.Entities
{
    public class BorrowedItem
    {
        public int Id { get; set; } //identificador unico
        public int MemberId { get; set; }
        public int LibraryItemId { get; set; }

        public Member? Member { get; set; }
        public LibraryItems? LibraryItem { get; set; }

    }
}
