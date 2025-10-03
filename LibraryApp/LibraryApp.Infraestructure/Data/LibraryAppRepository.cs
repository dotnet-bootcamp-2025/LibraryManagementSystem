using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Infraestructure.Data
{
    public class LibraryAppRepository : ILibraryAppRepository
    
    {
        private readonly AppDbContext _context;

        public LibraryAppRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddLibraryItem(LibraryItem libraryItem)
        {
            _context.LibraryItems.Add(libraryItem);
            _context.SaveChanges();

        }

        public IEnumerable<LibraryItem> GetAlLibraryItems()
        {
            //tolist enumera, materializar la consulta
            return _context.LibraryItems.ToList();
        }

        public LibraryItem? GetLibraryItemById(int id)
        {
            return _context.LibraryItems.Find(id);
        }

        public void updateLibraryItem(LibraryItem libraryItem)
        {
            _context.LibraryItems.Update(libraryItem);
            _context.SaveChanges();
        }

        public Member? GetMemberById(int id)
        {
            return _context.Members.Find(id);
        }

        public void AddBorrowedItem(BorrowedItem borrowedItem)
        {
            _context.BorrowedItems.Add(borrowedItem);
            _context.SaveChanges();
        }
    }
}
