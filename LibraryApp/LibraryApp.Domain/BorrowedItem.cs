namespace LibraryApp.Domain
{
    public class BorrowedItem
    {
        public int Id { get; }
        public int MemberId { get; }
        public int LibraryItemId { get; }
        public bool IsReturned { get; }

        public string BorrowedDate { get; }

        public BorrowedItem(int id, int memberId, int libraryItemId, bool isReturned, string borrowedDate)
        {
            Id = id;
            MemberId = memberId;
            LibraryItemId = libraryItemId;
            IsReturned = isReturned;
            BorrowedDate = borrowedDate;
        }
    }
}
