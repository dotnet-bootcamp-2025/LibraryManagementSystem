using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Abstractions
{
    public interface ILibraryAppRepository
    {
        IEnumerable<LibraryItem> GetAllLibraryItems();
        void AddLibraryItem(LibraryItem libraryItem);
        void UpdateLibraryItem(LibraryItem libraryItem);
        LibraryItem? GetLibraryItem(int id);
        Member? GetMemberById(int id);
        void AddBorrowedItem(BorrowedItem borrowedItem);
        void AddMember(Member member);
    }
}
