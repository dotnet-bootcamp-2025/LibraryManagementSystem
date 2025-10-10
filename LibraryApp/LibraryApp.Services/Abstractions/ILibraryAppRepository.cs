using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Abstractions
{
    public interface ILibraryAppRepository
    {
        IEnumerable<LibraryItem> GetAllLibraryItems();
        IEnumerable<Member> GetAllMembers();
        IEnumerable<BorrowedItem> GetBorrowedItemsFromMember(int memberId);
        void AddLibraryItem(LibraryItem libraryItem);
        void AddMember(Member member);
        void UpdateLibraryItem(LibraryItem libraryItem);
        LibraryItem? GetLibraryItemById(int id);
        Member? GetMemberById(int id);
        void AddBorrowedItem(BorrowedItem borrowedItem);
        void ReturnBorrowedItem(BorrowedItem borrowedItem);
    }
}
