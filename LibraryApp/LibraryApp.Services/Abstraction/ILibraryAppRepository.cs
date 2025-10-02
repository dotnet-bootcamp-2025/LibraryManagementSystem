using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Abstraction
{
    public interface ILibraryAppRepository
    {
        IEnumerable<LibraryItem> GetAllLibraryItems();
        void AddLibraryItem(LibraryItem libraryItem);
    }
}
