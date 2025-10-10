using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Abstractions
{
    public interface ILibraryAppRepository
    {
        IEnumerable<LibraryItem> GetAllLibraryItems();
        IEnumerable<Member> GetAllMembers();
        IEnumerable<LibraryItem> FindItems(string term);
        IEnumerable<BorrowedItem> GetMemberBorrowedItems(int memberId);

        Member? GetMemberById(int memberId);
        LibraryItem? GetItemById (int itemId);
        BorrowedItem? GetBorrowedItem(int memberId, int itemId);

        void AddLibraryItem(LibraryItem libraryItem);
        void AddMember(Member member);
        void AddBorrowedItem(BorrowedItem borrowedItem);

        void UpdateLibraryItem(LibraryItem libraryItem);
        void UpdateBorrowedItem(BorrowedItem borrowedItem);
        void SaveChanges();
    }
}
