using LibraryApp.Application.Abstractions;
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

        public void AddBorrowedItem(BorrowedItem borrowedItem)
        {
            _context.BorrowedItems.Add(borrowedItem);
            _context.SaveChanges();
        }

        public void AddLibraryItem(LibraryItem libraryItem)
        {
            _context.LibraryItems.Add(libraryItem);
            _context.SaveChanges();
        }

        public IEnumerable<LibraryItem> GetAllLibraryItems()
        {
            return _context.LibraryItems.ToList();
        }

        public LibraryItem? GetLibraryItem(int id)
        {
            return _context.LibraryItems.Find(id);

            // NOTA: Forma de traer varios registros
            //var x = _context.LibraryItems.Where(Li => Li.IsBorrowed && Li.Pages > 100).ToList();

            //return x;
        }

        public Member? GetMemberById(int id)
        {
            return _context.Members.Find(id);
        }

        public void UpdateLibraryItem(LibraryItem libraryItem)
        {
            _context.LibraryItems.Update(libraryItem);
            _context.SaveChanges();
        }
    }
}
