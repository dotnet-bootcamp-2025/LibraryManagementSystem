using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Abstractions
{
    public interface ILibraryAppRepository
    {
        IEnumerable<LibraryItem> GetAllLibraryItems();
        IEnumerable<LibraryItem> GetAllLibraryItemsByMemberId(int memberId);
        IEnumerable<Member> GetAllMembers();
        IEnumerable<BorrowedItem> GetBorrowedItemsByMember(int id);
        void AddLibraryItem(LibraryItem libraryItem);
        void UpdateLibraryItem(LibraryItem libraryItem);
        LibraryItem? GetLibraryItem(int id);
        Member? GetMemberById(int id);
        BorrowedItem? GetBorrowedItem(int memberId, int libraryItemId);
        void AddBorrowedItem(BorrowedItem borrowedItem);
        void ReturnBorrowedItem(int borrowedItemId);
        void AddMember(Member member);
    }
}
