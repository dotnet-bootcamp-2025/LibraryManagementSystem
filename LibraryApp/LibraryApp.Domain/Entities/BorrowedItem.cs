using System.ComponentModel.Design;

namespace LibraryApp.Domain.Entities
{
    public class BorrowedItem
    {
        public int Id { get; set; }
        public int LibraryItemId { get; set; }
        public int MemberId { get; set; }
        public DateTime BorrowDate { get; set; }

        public bool IsActive { get; set; } = true;

        //otras entities
        public Member? Member { get; set; }
        public LibraryItem? LibraryItem { get; set; }
    }
}
