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

        public IEnumerable<Domain.Entities.Member> GetAllMembers()
        {
            // Accede a la tabla de miembros y devuelve la lista de entidades
            return _context.Members.ToList();
        }

        public void AddMember(Domain.Entities.Member member)
        {
            _context.Members.Add(member);
            _context.SaveChanges(); // O manejar SaveChanges() en una unidad de trabajo.
        }

        public void AddBorrowedItem(BorrowedItem borrowedItem)
        {
            _context.BorrowedItems.Add(borrowedItem);
            _context.SaveChanges();
        }
                
        public Domain.Entities.BorrowedItem? GetBorrowedItem(int memberId, int libraryItemId)
        {
            return _context.BorrowedItems
                .FirstOrDefault(b => b.MemberId == memberId && b.LibraryItemId == libraryItemId);
        }
                
        public void RemoveBorrowedItem(Domain.Entities.BorrowedItem borrowedItem)
        {
            _context.BorrowedItems.Remove(borrowedItem);
            _context.SaveChanges();
        }
                
        public void UpdateLibraryItem(Domain.Entities.LibraryItem libraryItem)
        {
            _context.LibraryItems.Update(libraryItem);
            _context.SaveChanges();
        }
    }
}
