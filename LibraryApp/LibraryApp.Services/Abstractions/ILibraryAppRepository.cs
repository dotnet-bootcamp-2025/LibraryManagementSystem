using LibraryApp.Domain;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Abstractions
{
    public interface ILibraryAppRepository
    {
        IEnumerable<LibraryItems> GetAllItems();
        LibraryItem AddLibraryItem(LibraryItem book);
        LibraryItem AddMagazine(LibraryItem magazine);
        IEnumerable<LibraryItems> GetAllLibraryItems()
        {
            return _context.LibraryItems.ToList();
        }

        void UpdateLibraryItem(LibraryItems item);
        LibraryItems? GetLibraryItemById(int id)
        {
            return _context.LibraryItems.Find(id);
        }
        
        public void UpdateLibraryItem(LibraryItems item)
        {
            _context.LibraryItems.Update(item); // pendiente de checar todo este file
            _context.SaveChanges();
        }
    }
}
