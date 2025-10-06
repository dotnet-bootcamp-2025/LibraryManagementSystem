using LibraryApp.Application.Abstractions;
using LibraryApp.Domain;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Infrastructure.Data
{
    public class LibraryAppRepository : ILibraryAppRepository
    {
        private readonly AppDbContext _context;
        public LibraryAppRepository(AppDbContext context)
        {
            _context = context;
        }

        public LibraryItem AddLibraryItem(LibraryItem book)
        {
            throw new NotImplementedException();
        }

        public LibraryItem AddMagazine(LibraryItem magazine)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LibraryItems> GetAllItems()
        {
            return _context.LibraryItems.ToList();
        }

        public IEnumerable<LibraryItems> GetAllLibraryItems()
        {
            return _context.LibraryItems.ToList();
        }

        public LibraryItems? GetLibraryItemById(int id)
        {
            return _context.LibraryItems.Find(id);

            //var x = _context.LibraryItems.Where(li => li.IsBorrowed && li.Pages > 100).ToList();
            //return x;
        }

        public void UpdateLibraryItem(LibraryItems item)
        {
            throw new NotImplementedException();
        }
    }
}
