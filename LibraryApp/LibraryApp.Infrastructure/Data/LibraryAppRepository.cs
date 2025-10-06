using LibraryApp.Application.Abstraction;
using LibraryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Infrastructure.Data
{
    public class LibraryAppRepository : ILibraryAppRepository
    {
        private readonly AppDbContext _context;

        public LibraryAppRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddLibraryItem(LibraryItem item)
        {
            _context.LibraryItems.Add(item);
            _context.SaveChanges();
        }

        public void AddMember(Member member)
        {
            _context.Members.Add(member);
            _context.SaveChanges();
        }

        public IEnumerable<LibraryItem> GetAllLibraryItems()
        {
            return _context.LibraryItems.ToList();
        }

        public Member? GetMemberById(int id)
        {
            return _context.Members
                 .Include(member => member.BorrowedItems)
                 .ThenInclude(borrowedItem => borrowedItem.LibraryItem)
                 .FirstOrDefault(member => member.Id == id);
        }

        public LibraryItem? GetLibraryItemById(int id)
        {
            return _context.LibraryItems
                .FirstOrDefault(libraryItem => libraryItem.Id == id);
        }

        public void UpdateLibraryItem(LibraryItem LibraryItem)
        {
            _context.LibraryItems.Update(LibraryItem);
            _context.SaveChanges();
        }

        public void UpdateMember(Member member)
        {
            _context.Members.Update(member);
            _context.SaveChanges();
        }

        public void AddBorrowedItem(BorrowedItem borrowedItem)
        {
            _context.BorrowedItems.Add(borrowedItem);
            _context.SaveChanges();
        }

        public void ReturnItem(BorrowedItem borrowedItem)
        {
            _context.BorrowedItems.Update(borrowedItem);
            _context.SaveChanges();
        }

        public IEnumerable<Member> GetAllMembers()
        {
            return _context.Members
            .Include(member => member.BorrowedItems)
            .ThenInclude(borrowedItem => borrowedItem.LibraryItem)
            .ToList();
        }

        public IEnumerable<LibraryItem> FindItems(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return _context.LibraryItems.ToList();

            term = term.Trim();
            return _context.LibraryItems
                .Where(i => EF.Functions.Like(i.Title, $"%{term}%"))
                .ToList();
        }
    }
}