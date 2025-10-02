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

        public void AddLibraryItem(LibraryItem libraryItem)
        {
            _context.LibraryItems.Add(libraryItem);
            _context.SaveChanges();
        }

        public void AddMember(Member member)
        {
            _context.Members.Add(member);
            _context.SaveChanges();
        }

        public IEnumerable<LibraryItem> FindItems(string? term)
        {
            if (string.IsNullOrWhiteSpace(term)) return _context.LibraryItems.ToList();
            term = term.Trim().ToLowerInvariant();

            return _context.LibraryItems.
                Where(i => i.Title.ToLowerInvariant().Contains(term));
        }

        public IEnumerable<LibraryItem> GetAllLibraryItems()
        {
            return _context.LibraryItems.ToList();
        }

        public IEnumerable<Member> GetAllMembers()
        {
            return _context.Members.ToList();
        }
    }
}
