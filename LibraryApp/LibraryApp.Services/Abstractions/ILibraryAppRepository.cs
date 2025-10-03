using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Abstractions
{
    public interface ILibraryAppRepository
    {
        IEnumerable<LibraryItem> GetAlLibraryItems();

        void AddLibraryItem(LibraryItem libraryItem);

        void updateLibraryItem(LibraryItem libraryItem);

        LibraryItem? GetLibraryItemById(int id);

        Member? GetMemberById(int id);

        void AddBorrowedItem(BorrowedItem borrowedItem);
    }
}
