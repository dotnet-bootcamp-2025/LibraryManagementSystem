using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Abstractions
{
    public interface ILibraryAppRepository
    {
        IEnumerable<LibraryItem> GetAllLibraryItems();

        void AddLibraryItem(LibraryItem libraryItem);
        void UpdateLibraryItem(LibraryItem libraryItem);

        LibraryItem? GetLibraryItemById(int id);

        Member? GetMemberById(int id);
        void AddMember(Member member);

        void AddBorrowedItem(BorrowedItem borrowedItem);
        BorrowedItem? GetBorrowedItem(int memberId, int itemId);
        void RemoveBorrowedItem(BorrowedItem borrowedItem);
        IEnumerable<Member> GetAllMembers();
        IEnumerable<BorrowedItem> GetAllBorrowedItems();

    }
}