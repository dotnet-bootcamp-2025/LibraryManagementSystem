using LibraryApp.Domain;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Abstractions
{
    public interface ILibraryAppRepository
    {
        IEnumerable<LibraryItems> GetAllItems();
        LibraryItem AddLibraryItem(LibraryItem book);
        LibraryItem AddMagazine(LibraryItem magazine);
    }
}
