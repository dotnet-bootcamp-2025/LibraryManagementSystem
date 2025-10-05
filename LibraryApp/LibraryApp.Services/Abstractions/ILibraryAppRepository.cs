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

        void AddBorrowedItem(BorrowedItem borrowedItem);
        object GetBorrowedItem(int memberId, int itemId);
        void RemoveBorrowedItem(BorrowedItem borrowedItem);
        void RemoveBorrowedItem(object borrowedItem);
        IEnumerable<LibraryItem> FindLibraryItems(string? term);
        IEnumerable<Member> GetAllMembers();
        void AddMember(Member member);

    }
}
