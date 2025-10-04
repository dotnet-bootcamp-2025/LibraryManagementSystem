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
        public IEnumerable<LibraryItem> GetAllLibraryItems()
        {
            return _context.LibraryItems.ToList();
        }
        public IEnumerable<Member> GetAllMembers()
        {
            return _context.Members.Include(m => m.BorrowedItems).ThenInclude(b => b.LibraryItem).ToList();

        }
        public void RegisterMember(Member member)
        {
            _context.Members.Add(member);
            _context.SaveChanges();
        }
        public LibraryItem? GetLibraryItemById(int itemId)
        {
            return _context.LibraryItems.Find(itemId);
        }
        public void UpdateLibraryItem(LibraryItem libraryItem)
        {
            _context.LibraryItems.Update(libraryItem);
            _context.SaveChanges();
        }
        public Member? GetMemberById(int memberId)
        {
            return _context.Members.Find(memberId);
        }
        public void AddBorrowedItem(BorrowedItem borrowedItem)
        {
            _context.BorrowedItems.Add(borrowedItem);
            _context.SaveChanges();
        }
        public IEnumerable<BorrowedItem> GetBorrowedItem(int memberId, int itemId)
        {
            return _context.BorrowedItems.Where(b => b.MemberId == memberId && b.LibraryItemId == itemId).ToList();
        }

        public void RemoveBorrowedItem(BorrowedItem borrowedItem)
        {
            _context.BorrowedItems.Remove(borrowedItem);
            _context.SaveChanges();
        }
        public IEnumerable<LibraryItem> FindItems(string? term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return GetAllLibraryItems();
            }
            term = term.Trim().ToLower();
            return _context.LibraryItems.Where(i => i.Title.ToLower().Contains(term)
                                                || (i.Type == (int)Domain.Enums.LibraryItemTypeEnum.Book && i.Author != null && i.Author.ToLower().Contains(term))
                                                || (i.Type == (int)Domain.Enums.LibraryItemTypeEnum.Magazine && i.Publisher != null && i.Publisher.ToLower().Contains(term))
                                                ).ToList();
        }
    }
}
