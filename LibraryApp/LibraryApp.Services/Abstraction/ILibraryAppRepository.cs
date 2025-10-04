using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Abstraction
{
    public interface ILibraryAppRepository
    {
        IEnumerable<LibraryItem> GetAllLibraryItems();
        void AddLibraryItem(LibraryItem libraryItem);
        IEnumerable<Member> GetAllMembers();
        void RegisterMember(Member member);
        IEnumerable<LibraryItem> FindItems(string? term);
        LibraryItem? GetLibraryItemById(int itemId);
        Member? GetMemberById(int memberId);
        void AddBorrowedItem(BorrowedItem borrowedItem);
        void UpdateLibraryItem(LibraryItem libraryItem);
        IEnumerable<BorrowedItem> GetBorrowedItem(int memberId, int itemId);
        void RemoveBorrowedItem(BorrowedItem borrowedItem);
    }
}
