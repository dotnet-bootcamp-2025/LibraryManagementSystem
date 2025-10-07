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

        public IEnumerable<LibraryItems> GetAllLibraryItems()
        {
            return _context.LibraryItems.ToList();
        }

        void ILibraryAppRepository.AddLibraryItems(LibraryItems libraryItem)
        {
            _context.LibraryItems.Add(libraryItem);
            _context.SaveChanges();
        }

        public void UpdateLibraryItem(LibraryItems libraryItem)
        {
            _context.LibraryItems.Update(libraryItem);
            _context.SaveChanges();
        }

        public LibraryItems? GetLibraryItemById(int id)
        {
            return _context.LibraryItems.Find(id);

            //var x = _context.LibraryItems.Where(li => li.IsBorrowed && li.Pages > 100).Single();
            //return x;
        }


        public void AddBorrowedItem(BorrowedItem borrowedItem)
        {
            _context.BorrowedItems.Add(borrowedItem);
            _context.SaveChanges();
        }

        public IEnumerable<Domain.Entities.Member> GetAllMembers()
        {
            return _context.Members.ToList();
        }

        public void RemoveBorrowedItem(BorrowedItem borrowedItem)
        {
            _context.BorrowedItems.Remove(borrowedItem);
            _context.SaveChanges();
        }

        public Domain.Entities.Member? GetMemberById(int id)
        {
            return _context.Members.Find(id);
        }

        public void AddMember(Domain.Entities.Member member)
        {
            _context.Members.Add(member);
            _context.SaveChanges();
        }
    }
}
